using System.Net;
using System.Threading.Tasks;
using Anroo.Common;

namespace Anroo.MiLight
{
    public abstract class MLBulbController
    {
        protected struct MiLightGroupCommands
        {
            internal byte[] AllCommand;
            internal byte[] Group1Command;
            internal byte[] Group2Command;
            internal byte[] Group3Command;
            internal byte[] Group4Command;
        }

        protected static class GroupCommands
        {
#region Dual White
            internal static readonly MiLightGroupCommands DWOn = new MiLightGroupCommands
            {
                AllCommand = MLBulbCommands.DualWhite.AllOn,
                Group1Command = MLBulbCommands.DualWhite.Group1On,
                Group2Command = MLBulbCommands.DualWhite.Group2On,
                Group3Command = MLBulbCommands.DualWhite.Group3On,
                Group4Command = MLBulbCommands.DualWhite.Group4On,
            };
            internal static readonly MiLightGroupCommands DWOff = new MiLightGroupCommands
            {
                AllCommand = MLBulbCommands.DualWhite.AllOff,
                Group1Command = MLBulbCommands.DualWhite.Group1Off,
                Group2Command = MLBulbCommands.DualWhite.Group2Off,
                Group3Command = MLBulbCommands.DualWhite.Group3Off,
                Group4Command = MLBulbCommands.DualWhite.Group4Off,
            };
            internal static readonly MiLightGroupCommands DWNightMode = new MiLightGroupCommands
            {
                AllCommand = MLBulbCommands.DualWhite.AllNightMode,
                Group1Command = MLBulbCommands.DualWhite.Group1NightMode,
                Group2Command = MLBulbCommands.DualWhite.Group2NightMode,
                Group3Command = MLBulbCommands.DualWhite.Group3NightMode,
                Group4Command = MLBulbCommands.DualWhite.Group4NightMode,
            };

            internal static readonly MiLightGroupCommands DWFullBrightness = new MiLightGroupCommands
            {
                AllCommand = MLBulbCommands.DualWhite.AllFullBrightness,
                Group1Command = MLBulbCommands.DualWhite.Group1FullBrightness,
                Group2Command = MLBulbCommands.DualWhite.Group2FullBrightness,
                Group3Command = MLBulbCommands.DualWhite.Group3FullBrightness,
                Group4Command = MLBulbCommands.DualWhite.Group4FullBrightness,
            };
#endregion

#region RGBW

            internal static readonly MiLightGroupCommands RgbwOn = new MiLightGroupCommands
            {
                AllCommand = MLBulbCommands.Rgbw.AllOn,
                Group1Command = MLBulbCommands.Rgbw.Group1On,
                Group2Command = MLBulbCommands.Rgbw.Group2On,
                Group3Command = MLBulbCommands.Rgbw.Group3On,
                Group4Command = MLBulbCommands.Rgbw.Group4On,
            };
            internal static readonly MiLightGroupCommands RgbwOff = new MiLightGroupCommands
            {
                AllCommand = MLBulbCommands.Rgbw.AllOff,
                Group1Command = MLBulbCommands.Rgbw.Group1Off,
                Group2Command = MLBulbCommands.Rgbw.Group2Off,
                Group3Command = MLBulbCommands.Rgbw.Group3Off,
                Group4Command = MLBulbCommands.Rgbw.Group4Off,
            };
            internal static readonly MiLightGroupCommands RgbwWhiteMode = new MiLightGroupCommands
            {
                AllCommand = MLBulbCommands.Rgbw.AllWhite,
                Group1Command = MLBulbCommands.Rgbw.Group1White,
                Group2Command = MLBulbCommands.Rgbw.Group2White,
                Group3Command = MLBulbCommands.Rgbw.Group3White,
                Group4Command = MLBulbCommands.Rgbw.Group4White,
            };
            internal static readonly MiLightGroupCommands RgbwNightMode = new MiLightGroupCommands
            {
                AllCommand = MLBulbCommands.Rgbw.AllNight,
                Group1Command = MLBulbCommands.Rgbw.Group1Night,
                Group2Command = MLBulbCommands.Rgbw.Group2Night,
                Group3Command = MLBulbCommands.Rgbw.Group3Night,
                Group4Command = MLBulbCommands.Rgbw.Group4Night,
            };

#endregion
        }

        internal const int BulbControlPort = 8899;

        private readonly MLBulbGroupCode _defaultGroup;
        private readonly UdpTransmitter _client;

        protected MLBulbController(IPAddress bridgeIP, MLBulbGroupCode defaultGroup = MLBulbGroupCode.All)
        {
            _defaultGroup = defaultGroup;
            _client = new UdpTransmitter(new IPEndPoint(bridgeIP, BulbControlPort));
        }

        public abstract Task OnAsync(MLBulbGroupCode? group = null);
        public abstract Task OffAsync(MLBulbGroupCode? group = null);
        public abstract Task NightModeAsync(MLBulbGroupCode? group = null);

        protected async Task SendCommandToGroupAsync(MiLightGroupCommands command, MLBulbGroupCode? group = null)
        {
            switch (group.GetValueOrDefault(_defaultGroup))
            {
                case MLBulbGroupCode.All:
                    await SendCommandAsync(command.AllCommand);
                    break;
                case MLBulbGroupCode.One:
                    await SendCommandAsync(command.Group1Command);
                    break;
                case MLBulbGroupCode.Two:
                    await SendCommandAsync(command.Group2Command);
                    break;
                case MLBulbGroupCode.Three:
                    await SendCommandAsync(command.Group3Command);
                    break;
                case MLBulbGroupCode.Four:
                    await SendCommandAsync(command.Group4Command);
                    break;
            }
        }

        protected async Task SendCommandAsync(byte[] buffer)
        {
            await _client.SendDataAsync(buffer);
        }
    }
}