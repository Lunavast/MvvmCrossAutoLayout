﻿using System;
using UIKit;
using System.Drawing;
using System.Collections.Generic;
using Foundation;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using MvvmCrossAutoLayout.ViewModels;
using ObjCRuntime;
using MvvmCrossAutoLayout.iOS;
using CoreGraphics;

namespace AutoLayout
{

	[Register ("TicketView")]
	public class TicketView : MvxViewController
	{
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			NavigationController.NavigationBarHidden = true;
			View = BuildView ();
//			LocalNotificationHelper.SendLocalNotification ();
		}

		public string TelstraGreyL6 = "#9B9B9B";
		public float TowLabelTextSize = 12;
		public float TowDataTextSize = 20;

		private UIView BuildView ()
		{
			var Set = this.CreateBindingSet<TicketView, TicketViewModel> ();
			var Root = AutoLayoutContentView.CreateRoot ("Root", UIColor.DarkGray, "Helvetica-Bold");
			var ScrollView = Root.AddScrollView ("ScrollView", UIColor.DarkGray);

			var ContactBorder = ScrollView.AddContainer ("ContactBorder", UIColor.White);
			var PremisesBorder = ScrollView.AddContainer ("PremisesBorder", UIColor.White);
			ScrollView.AddConstraint ("V:|-16-[ContactBorder]-300-[PremisesBorder]-(>=4)-|");
			ScrollView.AddConstraint ("H:|-4-[ContactBorder]-4-|");
			ScrollView.AddConstraint ("H:|-4-[PremisesBorder]-4-|");

			SetCustomerAndContact (Set, ContactBorder);

			SetPremisesTabs (Set, PremisesBorder);

			Set.Apply ();

			return Root;
		}

		void SetCustomerAndContact (MvxFluentBindingDescriptionSet<TicketView, TicketViewModel> Set, AutoLayoutContentView ContactBorder)
		{
			var Customer = ContactBorder.AddContainer ("Customer", UIColor.White);
			var SiteContact = ContactBorder.AddContainer ("SiteContact", UIColor.White);
			ContactBorder.AddConstraint ("V:|-[Customer]-25-[SiteContact]|");
			ContactBorder.AddConstraint ("H:|-[Customer]-(>=8)-|");
			ContactBorder.AddConstraint ("H:|-[SiteContact]-(>=8)-|");
			SetCustomer (Set, Customer);
			SetContact (Set, SiteContact);
		}

		void SetCustomer (MvxFluentBindingDescriptionSet<TicketView, TicketViewModel> Set, AutoLayoutContentView Customer)
		{
			Customer.AddLabel ("CustomerLabel", "CUSTOMER", UIColor.LightGray/*TelstraGreyL6*/, TowLabelTextSize);
			var name = Customer.AddLabel ("CustomerName", "Alex Eadie", UIColor.Black, TowDataTextSize);
			var telstra = Customer.AddImage ("Customer_telstra_img", "ic_telstra_logo.png");
			Customer.AddConstraint ("V:|-18-[CustomerLabel]-7-[CustomerName]|");
			Customer.AddConstraint ("V:|-20-[Customer_telstra_img(28)]-(>=8)-|");
			Customer.AddConstraint ("H:|-19-[CustomerLabel]-(>=8)-|");
			Customer.AddConstraint ("H:|-19-[CustomerName]-(>=8)-|");
			Customer.AddConstraint ("H:|-(>=8)-[Customer_telstra_img(28)]-27-|");
			Set.Bind (name).To (vm => vm.Hello);
			Set.Bind (telstra).For ("Visibility").To (vm => vm.Telstra).WithConversion ("Visibility");
			
		}

		void SetContact (MvxFluentBindingDescriptionSet<TicketView, TicketViewModel> Set, AutoLayoutContentView SiteContact)
		{
			SiteContact.AddLabel ("SiteContactLabel", "SITE CONTACT", UIColor.LightGray/*TelstraGreyL6*/, TowLabelTextSize);
			var name = SiteContact.AddLabel ("SiteContactName", "Natasha Eadie", UIColor.Black, TowDataTextSize);
			var phone = SiteContact.AddLabel ("SiteContactPhone", "0412 123 456", UIColor.Black, TowDataTextSize);
			SiteContact.AddImage ("SitePhoneImage", "ic_phone.png");
			SiteContact.AddConstraint ("V:|[SiteContactLabel]-3-[SiteContactName]-3-[SiteContactPhone]-18-|");
			SiteContact.AddConstraint ("V:|-20-[SitePhoneImage(23)]-(>=8)-|");
			SiteContact.AddConstraint ("H:|-19-[SiteContactLabel]-(>=8)-|");
			SiteContact.AddConstraint ("H:|-19-[SiteContactName]-(>=10)-[SitePhoneImage(23)]-27-|");
			SiteContact.AddConstraint ("H:|-19-[SiteContactPhone]-(>=8)-|");
			Set.Bind (name).To (vm => vm.Hello);
			Set.Bind (phone).To (vm => vm.Hello);
		}


		static void SetPremisesTabs (MvxFluentBindingDescriptionSet<TicketView, TicketViewModel> Set, AutoLayoutContentView PremisesBorder)
		{
			var PremisesTabBox = PremisesBorder.AddContainer ("PremisesTabBox", UIColor.White);
			var ConnectionPillarTabBox = PremisesBorder.AddContainer ("ConnectionPillarTabBox", UIColor.LightGray);
			var ExchangeTabBox = PremisesBorder.AddContainer ("ExchangeTabBox", UIColor.LightGray);
			var PremisesLabel = PremisesTabBox.AddLabelCenteredXY ("PremisesTab", "PREMISES", UIColor.Black, 15);
			ConnectionPillarTabBox.AddLabelCenteredXY ("ConnectionPillarTab", "P74", UIColor.Blue, 15);
			ExchangeTabBox.AddLabelCenteredXY ("ExchangeTab", "PARR", UIColor.Blue, 15);

			Set.Bind (PremisesTabBox).For ("Tap").To (vm => vm.TestCommand).WithConversion ("CommandParameter", "alex");
			// https://github.com/MvvmCross/MvvmCross/wiki/Value-Converters
			Set.Bind (PremisesTabBox).For (field => field.BackgroundColor).To (vm => vm.PremisesTabBackgroundColor).WithConversion ("RGBA");
			Set.Bind (PremisesLabel).For (field => field.TextColor).To (vm => vm.PremisesTabTextColor).WithConversion ("RGBA");
			SetPremises (Set, PremisesBorder);
		}

		static void SetPremises (MvxFluentBindingDescriptionSet<TicketView, TicketViewModel> Set, AutoLayoutContentView PremisesBorder)
		{
			var address = PremisesBorder.AddLabel ("PremisesAddress", "UNIT 103, 402 DRAYTON-WELLCAMP RD PARRAMATTA", UIColor.Black, 15);
			Set.Bind (address).To (vm => vm.Hello);
			SetPremisesDirections (Set, PremisesBorder);
			PremisesBorder.AddConstraint ("V:|[PremisesTabBox(55)]-20-[PremisesAddress(60)]-18-|");
			PremisesBorder.AddConstraint ("V:|[ExchangeTabBox(55)]-20-[PremisesAddress(60)]-18-|");
			PremisesBorder.AddConstraint ("V:|[ConnectionPillarTabBox(55)]-20-[PremisesAddress(60)]-18-|");
			PremisesBorder.AddConstraint ("V:|[PremisesTabBox(55)]-28-[PremisesDirectionsBox]-(>=8)-|");
			PremisesBorder.AddConstraint ("H:|[PremisesTabBox]-2-[ConnectionPillarTabBox(==PremisesTabBox)]-2-[ExchangeTabBox(==PremisesTabBox)]|");
			PremisesBorder.AddConstraint ("H:|-19-[PremisesAddress(<=200)]-(>=6)-[PremisesDirectionsBox(66)]-10-|");
		}

		static void SetPremisesDirections (MvxFluentBindingDescriptionSet<TicketView, TicketViewModel> Set, AutoLayoutContentView PremisesBorder)
		{
			var PremisesDirectionsBox = PremisesBorder.AddContainer ("PremisesDirectionsBox", UIColor.White);
			PremisesDirectionsBox.AddLabelCenteredX ("PremisesDirections", "Directions", UIColor.Blue, 12);
			PremisesDirectionsBox.AddImageCenteredX ("PremisesDirectionsImage", "ic_directions.png");
			PremisesDirectionsBox.AddConstraint ("V:|[PremisesDirectionsImage(36)]-3-[PremisesDirections]|");
			PremisesDirectionsBox.AddConstraint ("H:|-(>=1)-[PremisesDirectionsImage(36)]-(>=1)-|");
			PremisesDirectionsBox.AddConstraint ("H:|-(>=1)-[PremisesDirections]-(>=1)-|");
		}

	}
}

