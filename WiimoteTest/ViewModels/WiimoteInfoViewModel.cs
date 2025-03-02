using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiimoteLib;

namespace WiimoteTest.ViewModels
{
    class WiimoteInfoViewModel
    {
        private Wiimote wm;
        public Wiimote Wm => wm;

        private WiimoteStateData wmStateData = new WiimoteStateData();
        public WiimoteStateData WmStateData => wmStateData;
        public event EventHandler WmStateDataChanged;
        public string DevicePath
        {
            get; set;
        } = string.Empty;

        private const int IR_CANVAS_WIDTH = 256;
        private const int IR_CANVAS_HEIGHT = 192;
        public int IRCanvasWidth
        {
            get; set;
        } = IR_CANVAS_WIDTH;
        public int IRCanvasHeight
        {
            get; set;
        } = IR_CANVAS_HEIGHT;

        private ReaderWriterLockSlim stateWriteLock = new ReaderWriterLockSlim();

        public WiimoteInfoViewModel(Wiimote wm)
        {
            this.wm = wm;
            DevicePath = wm.HIDDevicePath;

            if (wm.WiimoteState.ExtensionType != ExtensionType.BalanceBoard)
                wm.SetReportType(InputReport.IRExtensionAccel, IRSensitivity.Maximum, true);

            wm.WiimoteChanged += Wm_WiimoteChanged;
        }

        private void Wm_WiimoteChanged(object? sender, WiimoteChangedEventArgs e)
        {
            //stateWriteLock.EnterWriteLock();

            WiimoteState ws = e.WiimoteState;

            wmStateData.WiimoteA = ws.ButtonState.A;
            wmStateData.WiimoteB = ws.ButtonState.B;
            wmStateData.WiimoteMinus = ws.ButtonState.Minus;
            wmStateData.WiimoteHome = ws.ButtonState.Home;
            wmStateData.WiimotePlus = ws.ButtonState.Plus;
            wmStateData.WiimoteOne = ws.ButtonState.One;
            wmStateData.WiimoteTwo = ws.ButtonState.Two;
            wmStateData.WiimoteDpadUp = ws.ButtonState.Up;
            wmStateData.WiimoteDpadDown = ws.ButtonState.Down;
            wmStateData.WiimoteDpadLeft = ws.ButtonState.Left;
            wmStateData.WiimoteDpadRight = ws.ButtonState.Right;

            wmStateData.IR1X = ws.IRState.IRSensors[0].RawPosition.X;
            wmStateData.IR1Y = ws.IRState.IRSensors[0].RawPosition.Y;
            wmStateData.IR1Active = ws.IRState.IRSensors[0].Found;
            wmStateData.IR1EllipseTop = (int)((IRCanvasHeight + 3 + 3) * (wmStateData.IR1Y / 767.0) - 3);
            wmStateData.IR1EllipseLeft = (int)((-3 - IRCanvasWidth + 3) * (wmStateData.IR1X / 1023.0) + (IRCanvasWidth + 3));

            wmStateData.IR2X = ws.IRState.IRSensors[1].RawPosition.X;
            wmStateData.IR2Y = ws.IRState.IRSensors[1].RawPosition.Y;
            wmStateData.IR2Active = ws.IRState.IRSensors[1].Found;
            wmStateData.IR2EllipseTop = (int)((IRCanvasHeight + 3 + 3) * (wmStateData.IR2Y / 767.0) - 3);
            wmStateData.IR2EllipseLeft = (int)((-3 - IRCanvasWidth + 3) * (wmStateData.IR2X / 1023.0) + (IRCanvasWidth + 3));

            wmStateData.IR3X = ws.IRState.IRSensors[2].RawPosition.X;
            wmStateData.IR3Y = ws.IRState.IRSensors[2].RawPosition.Y;
            wmStateData.IR3Active = ws.IRState.IRSensors[2].Found;
            wmStateData.IR3EllipseTop = (int)((IRCanvasHeight + 3 + 3) * (wmStateData.IR3Y / 767.0) - 3);
            wmStateData.IR3EllipseLeft = (int)((-3 - IRCanvasWidth + 3) * (wmStateData.IR3X / 1023.0) + (IRCanvasWidth + 3));

            wmStateData.IR4X = ws.IRState.IRSensors[3].RawPosition.X;
            wmStateData.IR4Y = ws.IRState.IRSensors[3].RawPosition.Y;
            wmStateData.IR4Active = ws.IRState.IRSensors[3].Found;
            wmStateData.IR4EllipseTop = (int)((IRCanvasHeight + 3 + 3) * (wmStateData.IR4Y / 767.0) - 3);
            wmStateData.IR4EllipseLeft = (int)((-3 - IRCanvasWidth + 3) * (wmStateData.IR4X / 1023.0) + (IRCanvasWidth + 3));

            wmStateData.IRMidActive = wmStateData.IR1Active && wmStateData.IR2Active;
            if (wmStateData.IRMidActive)
            {
                wmStateData.IRMidEllipseTop = (int)((IRCanvasHeight + 3 + 3) * (ws.IRState.RawMidpoint.Y / 767.0) - 3);
                wmStateData.IRMidEllipseLeft = (int)((-3 - IRCanvasWidth + 3) * (ws.IRState.RawMidpoint.X / 1023.0) + (IRCanvasWidth + 3));
            }

            wmStateData.IRMidX = ws.IRState.RawMidpoint.X;
            wmStateData.IRMidY = ws.IRState.RawMidpoint.Y;
            wmStateData.IRMidNormX = ws.IRState.Midpoint.X;
            wmStateData.IRMidNormY = ws.IRState.Midpoint.Y;
            wmStateData.IRMidStr = ws.IRState.RawMidpoint.ToString();
            wmStateData.IRMidNormStr = ws.IRState.Midpoint.ToString();

            wmStateData.Battery = ws.Battery;
            wmStateData.WiimoteAccelValues = ws.AccelState.Values.ToString();

            switch(ws.ExtensionType)
            {
                case ExtensionType.Nunchuk:
                    wmStateData.NunchukAccelValues = ws.NunchukState.AccelState.Values.ToString();
                    wmStateData.NunchukJoystickValues = ws.NunchukState.Joystick.ToString();
                    wmStateData.NunchukRawJoystickValues = ws.NunchukState.RawJoystick.ToString();
                    wmStateData.NunchukC = ws.NunchukState.C;
                    wmStateData.NunchukZ = ws.NunchukState.Z;

                    wmStateData.NunchukJoystickXCalib = $"Max X: {ws.NunchukState.CalibrationInfo.MaxX} | Min X: {ws.NunchukState.CalibrationInfo.MinX} | Mid X: {ws.NunchukState.CalibrationInfo.MidX}";
                    wmStateData.NunchukJoystickYCalib = $"Max Y: {ws.NunchukState.CalibrationInfo.MaxY} | Min Y: {ws.NunchukState.CalibrationInfo.MinY} | Mid Y: {ws.NunchukState.CalibrationInfo.MidY}";

                    break;

                default: break;
            }

            // Tell controls that state has changed. Re-evaluate bindings
            WmStateDataChanged?.Invoke(this, EventArgs.Empty);

            //stateWriteLock.ExitWriteLock();
        }

        public void TearDown()
        {
            wm.WiimoteChanged -= Wm_WiimoteChanged;

            wm.Disconnect();
        }
    }

    class WiimoteStateData
    {
        public bool WiimoteA
        {
            get; set;
        }

        public bool WiimoteB
        {
            get; set;
        }

        public bool WiimoteMinus
        {
            get; set;
        }

        public bool WiimoteHome
        {
            get; set;
        }

        public bool WiimotePlus
        {
            get; set;
        }

        public bool WiimoteOne
        {
            get; set;
        }

        public bool WiimoteTwo
        {
            get; set;
        }

        public bool WiimoteDpadUp
        {
            get; set;
        }

        public bool WiimoteDpadDown
        {
            get; set;
        }

        public bool WiimoteDpadLeft
        {
            get; set;
        }

        public bool WiimoteDpadRight
        {
            get; set;
        }

        public int IR1X
        {
            get; set;
        } = 0;

        public int IR1Y
        {
            get; set;
        } = 0;

        public string IR1
        {
            get => $"{IR1X} {IR1Y}";
        }

        public bool IR1Active
        {
            get; set;
        }

        public int IR1EllipseTop
        {
            get; set;
        }

        public int IR1EllipseLeft
        {
            get; set;
        }

        public int IR2X
        {
            get; set;
        } = 0;

        public int IR2Y
        {
            get; set;
        } = 0;

        public string IR2
        {
            get => $"{IR2X} {IR2Y}";
        }

        public bool IR2Active
        {
            get; set;
        }

        public int IR2EllipseTop
        {
            get; set;
        }

        public int IR2EllipseLeft
        {
            get; set;
        }

        public int IR3X
        {
            get; set;
        } = 0;

        public int IR3Y
        {
            get; set;
        } = 0;

        public string IR3
        {
            get => $"{IR3X} {IR3Y}";
        }

        public bool IR3Active
        {
            get; set;
        }

        public int IR3EllipseTop
        {
            get; set;
        }

        public int IR3EllipseLeft
        {
            get; set;
        }

        public int IR4X
        {
            get; set;
        } = 0;

        public int IR4Y
        {
            get; set;
        } = 0;

        public string IR4
        {
            get => $"{IR4X} {IR4Y}";
        }

        public bool IR4Active
        {
            get; set;
        }

        public int IR4EllipseTop
        {
            get; set;
        }

        public int IR4EllipseLeft
        {
            get; set;
        }

        public bool IRMidActive
        {
            get; set;
        }

        public int IRMidEllipseTop
        {
            get; set;
        }

        public int IRMidEllipseLeft
        {
            get; set;
        }

        public int IRMidX
        {
            get; set;
        }

        public int IRMidY
        {
            get; set;
        }

        public double IRMidNormX
        {
            get; set;
        }

        public double IRMidNormY
        {
            get; set;
        }

        public string IRMidStr
        {
            get; set;
        } = string.Empty;

        public string IRMidNormStr
        {
            get; set;
        } = string.Empty;

        public string WiimoteAccelValues
        {
            get; set;
        } = string.Empty;

        public string NunchukAccelValues
        {
            get; set;
        } = string.Empty;

        public string NunchukJoystickValues
        {
            get; set;
        } = string.Empty;

        public string NunchukRawJoystickValues
        {
            get; set;
        } = string.Empty;

        public bool NunchukC
        {
            get; set;
        }

        public bool NunchukZ
        {
            get; set;
        }

        public string NunchukJoystickXCalib
        {
            get; set;
        } = string.Empty;

        public string NunchukJoystickYCalib
        {
            get; set;
        } = string.Empty;

        public double Battery
        {
            get; set;
        }

        public string BatteryStr
        {
            get => $"{Battery:N2}%";
        }
    }
}
