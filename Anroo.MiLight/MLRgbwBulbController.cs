using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Anroo.MiLight
{
    public class MLRgbwBulbController : MLBulbController
    {
        public MLRgbwBulbController(IPAddress bridgeIP, ProtocolType protocol, MLBulbGroupCode defaultGroup = MLBulbGroupCode.All) : base(bridgeIP, protocol, defaultGroup)
        {
        }

        public override async Task OnAsync(MLBulbGroupCode? group = null)
        {
            await SendCommandToGroupAsync(GroupCommands.RgbwOn, group);
        }

        public override async Task OffAsync(MLBulbGroupCode? group = null)
        {
            await SendCommandToGroupAsync(GroupCommands.RgbwOff, group);
        }

        public override async Task NightModeAsync(MLBulbGroupCode? group = null)
        {
            await SendCommandToGroupAsync(GroupCommands.RgbwOff, @group);
            await SendCommandToGroupAsync(GroupCommands.RgbwNightMode, @group);
        }

        public async Task DiscoModeAsync(MLBulbGroupCode? group = null)
        {
            await SendCommandToGroupAsync(GroupCommands.RgbwOn, group);
            await SendCommandAsync(BulbCommands.Rgbw.DiscoMode);
        }

        public async Task DiscoSpeedSlowerAsync(MLBulbGroupCode? group = null)
        {
            await SendCommandToGroupAsync(GroupCommands.RgbwOn, group);
            await SendCommandAsync(BulbCommands.Rgbw.DiscoSpeedSlower);
        }

        public async Task DiscoSpeedFasterAsync(MLBulbGroupCode? group = null)
        {
            await SendCommandToGroupAsync(GroupCommands.RgbwOn, group);
            await SendCommandAsync(BulbCommands.Rgbw.DiscoSpeedFaster);
        }

        public async Task WhiteModeAsync(MLBulbGroupCode? group = null)
        {
            await SendCommandToGroupAsync(GroupCommands.RgbwOn, group);
            await SendCommandToGroupAsync(GroupCommands.RgbwWhiteMode, group);
        }

        public async Task BrightnessAsync(byte level, MLBulbGroupCode? group = null)
        {
            if (level < MLDWBulbController.MinBrightness)
            {
                level = MLDWBulbController.MinBrightness;
            }
            else if (level > MLDWBulbController.MaxBrightness)
            {
                level = MLDWBulbController.MaxBrightness;
            }

            var cmd = (byte[])BulbCommands.Rgbw.Brightness.Clone();
            cmd[1] = level;
            await SendCommandToGroupAsync(GroupCommands.RgbwOn, group);
            await SendCommandAsync(cmd);
        }

        public async Task ColorAsync(byte color, MLBulbGroupCode? group = null)
        {
            var cmd = (byte[])BulbCommands.Rgbw.Color.Clone();
            cmd[1] = color;
            await SendCommandToGroupAsync(GroupCommands.RgbwOn, group);
            await SendCommandAsync(cmd);
        }
    }
}