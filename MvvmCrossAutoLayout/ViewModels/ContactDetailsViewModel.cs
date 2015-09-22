using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore.UI;

namespace MvvmCrossAutoLayout.ViewModels
{
	public class ContactDetailsViewModel 
		: MvxViewModel
	{
		public MvxCommand<string> TestCommand { 
			get { return new MvxCommand<string> (Test); } 
		}

		private bool _Telstra = true;

		public bool Telstra {
			get { return _Telstra; }
			set { RaisePropertyChanged (() => Telstra); }
		}



		private MvxColor _BackgroundColor = MvxColors.Cornsilk;

		public MvxColor BackgroundColor {
			get { return _BackgroundColor; }
			set { RaisePropertyChanged (() => BackgroundColor); }
		}

		private MvxColor _TextColor = MvxColors.Cyan;

		public MvxColor TextColor {
			get { return _TextColor; }
			set { RaisePropertyChanged (() => TextColor); }
		}


		private string _hello = "Hello Tocklet";

		public string Hello { 
			get { return _hello; }
			set {
				_hello = value;
				RaisePropertyChanged (() => Hello);
			}
		}

		public void Test (string commandParameter)
		{
			// Don't call the individual set methods as the action of firing RaisePropertyChanged for each one
			// causes some kind of race condition;
			_hello = commandParameter;
			_Telstra = !Telstra;
			RaiseAllPropertiesChanged ();
		}
	}
}
