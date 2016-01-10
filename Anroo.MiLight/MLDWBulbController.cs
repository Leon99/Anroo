using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Anroo.MiLight
{
    public class MLDWBulbController : MLBulbController
    {
        public const int MaxBrightness = 27;
        public const int MinBrightness = 2;

        public MLDWBulbController(IPAddress bridgeIP, MLBulbGroupCode defaultGroup = MLBulbGroupCode.All) : base(bridgeIP, defaultGroup)
        {
        }

        public override async Task OnAsync(MLBulbGroupCode? group = null)
        {
            await SendCommandToGroupAsync(GroupCommands.DWOn, group);
        }

        public override async Task OffAsync(MLBulbGroupCode? group = null)
        {
            await SendCommandToGroupAsync(GroupCommands.DWOff, group);
        }

        public override async Task NightModeAsync(MLBulbGroupCode? group = null)
        {
            await SendCommandToGroupAsync(GroupCommands.DWOff, group);
            await SendCommandToGroupAsync(GroupCommands.DWNightMode, group);
        }

        public async Task FullBrightnessAsync(MLBulbGroupCode? group = null)
        {
            await SendCommandToGroupAsync(GroupCommands.DWFullBrightness, group);
        }

        public async Task BrightnessUpAsync(MLBulbGroupCode? group = null)
        {
            await SendCommandToGroupAsync(GroupCommands.DWOn, group);
            await SendCommandAsync(BulbCommands.DualWhite.BrightnessUp);
        }

        public async Task BrightnessDownAsync(MLBulbGroupCode? group = null)
        {
            await SendCommandToGroupAsync(GroupCommands.DWOn, group);
            await SendCommandAsync(BulbCommands.DualWhite.BrightnessDown);
        }

        public async Task ColorTemperatureUpAsync(MLBulbGroupCode? group = null)
        {
            await SendCommandToGroupAsync(GroupCommands.DWOn, group);
            await SendCommandAsync(BulbCommands.DualWhite.ColorTemperatureUp);
        }

        public async Task ColorTemperatureDownAsync(MLBulbGroupCode? group = null)
        {
            await SendCommandToGroupAsync(GroupCommands.DWOn, group);
            await SendCommandAsync(BulbCommands.DualWhite.ColorTemperatureDown);
        }

        public async Task DWBrightnessFadeDownAsync(MLBulbGroupCode? group = null, int delay = 1000, CancellationToken? ct = null)
        {
            await SendCommandToGroupAsync(GroupCommands.DWOn, group);
            for (int i = 1; i < 10; i++)
            {
                if ((ct != null) && ct.Value.CanBeCanceled && ct.Value.IsCancellationRequested)
                    break;
                await SendCommandAsync(BulbCommands.DualWhite.BrightnessDown);
            }
        }

        public async Task DWBrightnessFadeUpAsync(MLBulbGroupCode? group = null, int delay = 1000, CancellationToken? ct = null)
        {
            await SendCommandToGroupAsync(GroupCommands.DWOn, group);
            for (int i = 1; i < 10; i++)
            {
                if ((ct != null) && ct.Value.CanBeCanceled && ct.Value.IsCancellationRequested)
                    break;
                await SendCommandAsync(BulbCommands.DualWhite.BrightnessUp);
            }
        }
    }
}