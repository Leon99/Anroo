using System;
using System.Net;
using System.Threading.Tasks;
using Anroo.Common.Udp;

namespace Anroo.MiLight
{
    public abstract class MLBulbController : IDisposable
    {
        protected struct MiLightGroupCommands
        {
            internal byte[] AllCommand;
            internal byte[] Group1Command;
            internal byte[] Group2Command;
            internal byte[] Group3Command;
            internal byte[] Group4Command;

            internal bool Repeatable;
        }

        protected static class GroupCommands
        {
#region Dual White
            internal static readonly MiLightGroupCommands DWOn = new MiLightGroupCommands
            {
                AllCommand = BulbCommands.DualWhite.AllOn,
                Group1Command = BulbCommands.DualWhite.Group1On,
                Group2Command = BulbCommands.DualWhite.Group2On,
                Group3Command = BulbCommands.DualWhite.Group3On,
                Group4Command = BulbCommands.DualWhite.Group4On,
                Repeatable = true,
            };
            internal static readonly MiLightGroupCommands DWOff = new MiLightGroupCommands
            {
                AllCommand = BulbCommands.DualWhite.AllOff,
                Group1Command = BulbCommands.DualWhite.Group1Off,
                Group2Command = BulbCommands.DualWhite.Group2Off,
                Group3Command = BulbCommands.DualWhite.Group3Off,
                Group4Command = BulbCommands.DualWhite.Group4Off,
                Repeatable = true,
            };
            internal static readonly MiLightGroupCommands DWNightMode = new MiLightGroupCommands
            {
                AllCommand = BulbCommands.DualWhite.AllNightMode,
                Group1Command = BulbCommands.DualWhite.Group1NightMode,
                Group2Command = BulbCommands.DualWhite.Group2NightMode,
                Group3Command = BulbCommands.DualWhite.Group3NightMode,
                Group4Command = BulbCommands.DualWhite.Group4NightMode,
            };

            internal static readonly MiLightGroupCommands DWFullBrightness = new MiLightGroupCommands
            {
                AllCommand = BulbCommands.DualWhite.AllFullBrightness,
                Group1Command = BulbCommands.DualWhite.Group1FullBrightness,
                Group2Command = BulbCommands.DualWhite.Group2FullBrightness,
                Group3Command = BulbCommands.DualWhite.Group3FullBrightness,
                Group4Command = BulbCommands.DualWhite.Group4FullBrightness,
                Repeatable = true,
            };
            #endregion

            #region RGBW

            internal static readonly MiLightGroupCommands RgbwOn = new MiLightGroupCommands
            {
                AllCommand = BulbCommands.Rgbw.AllOn,
                Group1Command = BulbCommands.Rgbw.Group1On,
                Group2Command = BulbCommands.Rgbw.Group2On,
                Group3Command = BulbCommands.Rgbw.Group3On,
                Group4Command = BulbCommands.Rgbw.Group4On,
                Repeatable = true,
            };
            internal static readonly MiLightGroupCommands RgbwOff = new MiLightGroupCommands
            {
                AllCommand = BulbCommands.Rgbw.AllOff,
                Group1Command = BulbCommands.Rgbw.Group1Off,
                Group2Command = BulbCommands.Rgbw.Group2Off,
                Group3Command = BulbCommands.Rgbw.Group3Off,
                Group4Command = BulbCommands.Rgbw.Group4Off,
                Repeatable = true,
            };
            internal static readonly MiLightGroupCommands RgbwWhiteMode = new MiLightGroupCommands
            {
                AllCommand = BulbCommands.Rgbw.AllWhite,
                Group1Command = BulbCommands.Rgbw.Group1White,
                Group2Command = BulbCommands.Rgbw.Group2White,
                Group3Command = BulbCommands.Rgbw.Group3White,
                Group4Command = BulbCommands.Rgbw.Group4White,
                Repeatable = true,
            };
            internal static readonly MiLightGroupCommands RgbwNightMode = new MiLightGroupCommands
            {
                AllCommand = BulbCommands.Rgbw.AllNight,
                Group1Command = BulbCommands.Rgbw.Group1Night,
                Group2Command = BulbCommands.Rgbw.Group2Night,
                Group3Command = BulbCommands.Rgbw.Group3Night,
                Group4Command = BulbCommands.Rgbw.Group4Night,
            };

#endregion
        }

        internal const int BulbControlPort = 8899;

        private readonly MLBulbGroupCode _defaultGroup;
        private readonly UdpTransmitter _transmitter;

        protected MLBulbController(IPAddress bridgeIP, MLBulbGroupCode defaultGroup = MLBulbGroupCode.All)
        {
            _defaultGroup = defaultGroup;
            _transmitter = new UdpTransmitter(new IPEndPoint(bridgeIP, BulbControlPort));
        }

        public abstract Task OnAsync(MLBulbGroupCode? group = null);
        public abstract Task OffAsync(MLBulbGroupCode? group = null);
        public abstract Task NightModeAsync(MLBulbGroupCode? group = null);

        protected async Task SendCommandToGroupAsync(MiLightGroupCommands command, MLBulbGroupCode? group = null)
        {
            switch (group.GetValueOrDefault(_defaultGroup))
            {
                case MLBulbGroupCode.All:
                    await SendCommandAsync(command.AllCommand, command.Repeatable);
                    break;
                case MLBulbGroupCode.One:
                    await SendCommandAsync(command.Group1Command, command.Repeatable);
                    break;
                case MLBulbGroupCode.Two:
                    await SendCommandAsync(command.Group2Command, command.Repeatable);
                    break;
                case MLBulbGroupCode.Three:
                    await SendCommandAsync(command.Group3Command, command.Repeatable);
                    break;
                case MLBulbGroupCode.Four:
                    await SendCommandAsync(command.Group4Command, command.Repeatable);
                    break;
            }
        }

        protected async Task SendCommandAsync(byte[] buffer, bool repeatable = false)
        {
            await _transmitter.SendDataAsync(buffer, repeatable ? 4 : 1);
        }

        public void Dispose()
        {
            _transmitter.Dispose();
        }
    }
}