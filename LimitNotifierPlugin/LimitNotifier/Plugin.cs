using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using System;

namespace LimitNotifier;

public sealed class LimitNotifier : IDalamudPlugin
{
    public string Name => "Limit Notifier";
    
    [PluginService]
    private IClientState ClientState { get; init; } = null!;
    
    [PluginService]
    private IFramework Framework { get; init; } = null!;
    
    [PluginService]
    private IToastGui ToastGui { get; init; } = null!;
    
    [PluginService]
    private IPluginLog PluginLog { get; init; } = null!;
    
    [PluginService]
    private ICommandManager CommandManager { get; init; } = null!;
    
    private int previousLimitBreakUnits = 0;
    private bool hasNotifiedBar1 = false;
    private bool hasNotifiedBar2 = false;
    private bool hasNotifiedBar3 = false;

    // Limit Break bar units: typically 1000 units per bar
    private const int BAR_1_THRESHOLD = 1000;
    private const int BAR_2_THRESHOLD = 2000;
    private const int BAR_3_THRESHOLD = 3000;

    public LimitNotifier()
    {
        Framework.Update += OnFrameworkUpdate;
        
        // Register test commands
        CommandManager.AddHandler("/limitnotifiertest", new CommandInfo(OnTestCommand)
        {
            HelpMessage = "Test Limit Notifier toasts. Usage: /limitnotifiertest [1|2|3] - Shows a toast for the specified number of bars (default: 1)"
        });
        
        CommandManager.AddHandler("/lnbtest", new CommandInfo(OnTestCommand)
        {
            HelpMessage = "Short alias for /limitnotifiertest"
        });
    }

    private unsafe void OnFrameworkUpdate(IFramework framework)
    {
        if (ClientState.LocalPlayer == null)
        {
            // Reset notifications when player is not in game
            hasNotifiedBar1 = false;
            hasNotifiedBar2 = false;
            hasNotifiedBar3 = false;
            previousLimitBreakUnits = 0;
            return;
        }

        try
        {
            // Get Limit Break controller instance
            var lbController = LimitBreakController.Instance();
            if (lbController == null)
                return;

            // Get current Limit Break units (typically 1000 per bar)
            int currentLimitBreakUnits = lbController->CurrentUnits;
            int barUnits = lbController->BarUnits; // Units per bar (usually 1000)
            int barCount = lbController->BarCount; // Number of bars available (1, 2, or 3)

            // Calculate thresholds based on actual bar units
            int bar1Threshold = barUnits;
            int bar2Threshold = barUnits * 2;
            int bar3Threshold = barUnits * 3;

            // Only check thresholds if we have enough bars available
            if (barCount >= 1 && currentLimitBreakUnits >= bar1Threshold && previousLimitBreakUnits < bar1Threshold && !hasNotifiedBar1)
            {
                ShowToast("Limit Break: 1 Bar Ready!");
                hasNotifiedBar1 = true;
            }

            if (barCount >= 2 && currentLimitBreakUnits >= bar2Threshold && previousLimitBreakUnits < bar2Threshold && !hasNotifiedBar2)
            {
                ShowToast("Limit Break: 2 Bars Ready!");
                hasNotifiedBar2 = true;
            }

            if (barCount >= 3 && currentLimitBreakUnits >= bar3Threshold && previousLimitBreakUnits < bar3Threshold && !hasNotifiedBar3)
            {
                ShowToast("Limit Break: 3 Bars Ready!");
                hasNotifiedBar3 = true;
            }

            // Reset notifications if gauge drops below thresholds (e.g., after using LB)
            if (currentLimitBreakUnits < bar1Threshold)
            {
                hasNotifiedBar1 = false;
            }
            if (currentLimitBreakUnits < bar2Threshold)
            {
                hasNotifiedBar2 = false;
            }
            if (currentLimitBreakUnits < bar3Threshold)
            {
                hasNotifiedBar3 = false;
            }

            previousLimitBreakUnits = currentLimitBreakUnits;
        }
        catch (Exception ex)
        {
            // Silently handle errors to prevent plugin crashes
            PluginLog.Error($"Error in LimitNotifier update: {ex.Message}");
        }
    }

    private void ShowToast(string message)
    {
        // Use Dalamud's toast notification system with custom styling
        // Using purple/magenta theme similar to the user's other plugin
        ToastGui.ShowNormal(message);
    }

    private void OnTestCommand(string command, string args)
    {
        // Parse argument to determine which bar to test (default: 1)
        var barNumber = 1;
        if (!string.IsNullOrWhiteSpace(args))
        {
            if (int.TryParse(args.Trim(), out var parsed) && parsed >= 1 && parsed <= 3)
            {
                barNumber = parsed;
            }
        }

        // Show the appropriate test toast
        switch (barNumber)
        {
            case 1:
                ShowToast("Limit Break: 1 Bar Ready!");
                PluginLog.Info("Test toast sent: 1 Bar");
                break;
            case 2:
                ShowToast("Limit Break: 2 Bars Ready!");
                PluginLog.Info("Test toast sent: 2 Bars");
                break;
            case 3:
                ShowToast("Limit Break: 3 Bars Ready!");
                PluginLog.Info("Test toast sent: 3 Bars");
                break;
        }
    }

    public void Dispose()
    {
        Framework.Update -= OnFrameworkUpdate;
        CommandManager.RemoveHandler("/limitnotifiertest");
        CommandManager.RemoveHandler("/lnbtest");
    }
}

