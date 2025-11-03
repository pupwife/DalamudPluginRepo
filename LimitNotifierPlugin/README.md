# Limit Notifier

A Dalamud plugin for Final Fantasy XIV that displays toast notifications when the Limit Break bar reaches 1, 2, or 3 full bars.

## Features

- Shows a toast notification when Limit Break reaches 1 bar (1000 gauge)
- Shows a toast notification when Limit Break reaches 2 bars (2000 gauge)
- Shows a toast notification when Limit Break reaches 3 bars (3000 gauge)
- Automatically resets notifications when the gauge drops below thresholds

## Installation

### Prerequisites

1. **XIVLauncher**: This plugin requires XIVLauncher with Dalamud enabled
   - Download from: https://github.com/goatcorp/FFXIVQuickLauncher
   - Ensure Dalamud is enabled in XIVLauncher settings

2. **Development Setup** (if building from source):
   - .NET 9.0 SDK
   - Visual Studio 2022, Rider, or another IDE with C# support

### Building from Source

1. Clone or download this repository
2. Open the project in your IDE
3. Restore NuGet packages
4. Build the project (this will create a DLL in `bin/Debug/net9.0/` or `bin/Release/net9.0/`)
5. Copy the built DLL and `plugin.json` to your Dalamud plugins folder:
   - Default location: `%AppData%\XIVLauncher\pluginConfigs\LimitNotifier\`
   - Or use Dalamud's dev plugin loader to load it directly

### Installation via Dalamud Plugin Repository

If this plugin is published to the Dalamud plugin repository, you can install it directly from the plugin installer in-game:

1. Type `/xlsettings` in-game to open Dalamud settings
2. Go to the "Experimental" tab
3. Enable "Show testing plugins" if needed
4. Search for "Limit Notifier" and click Install

## Usage

Once installed, the plugin will automatically monitor your Limit Break gauge during combat and display toast notifications when it reaches each threshold. No configuration is needed.

The notifications will appear as toast messages in the game when:
- **1 Bar**: Limit Break gauge reaches 1000
- **2 Bars**: Limit Break gauge reaches 2000  
- **3 Bars**: Limit Break gauge reaches 3000

Notifications reset automatically if the gauge drops below a threshold (for example, after using a Limit Break).

### Test Commands

You can test the toast notifications without waiting for Limit Break to fill up using these commands:

- `/limitnotifiertest` or `/lnbtest` - Shows a test toast for 1 bar (default)
- `/limitnotifiertest 1` or `/lnbtest 1` - Shows a test toast for 1 bar
- `/limitnotifiertest 2` or `/lnbtest 2` - Shows a test toast for 2 bars
- `/limitnotifiertest 3` or `/lnbtest 3` - Shows a test toast for 3 bars

These commands are useful for testing the notification appearance without having to run dungeons or trials repeatedly.

## Troubleshooting

- If notifications aren't appearing, ensure Dalamud is enabled in XIVLauncher
- Check that you're in a party/alliance when testing (Limit Break is only available in groups)
- Ensure the plugin is enabled in `/xlplugins`

## Development

This plugin uses:
- **Dalamud.Plugin** - Core Dalamud plugin framework
- **IPartyList** - Access to party data including Limit Break gauge
- **IToastGui** - Toast notification system

The plugin monitors the Limit Break gauge every frame and triggers notifications when crossing threshold values.

## License

This plugin is provided as-is. Use at your own discretion and in accordance with Square Enix's Terms of Service.

## Credits

Created for Final Fantasy XIV using the Dalamud plugin framework.

