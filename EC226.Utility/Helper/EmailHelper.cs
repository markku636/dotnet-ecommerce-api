using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.Utility.Helper
{
   public static class EmailHelper
   {
      private static readonly Dictionary<string, Dictionary<string, string>> Localizations = new Dictionary<string, Dictionary<string, string>>
      {
         ["en"] = new Dictionary<string, string>
         {
            ["WelcomeToYourShop"] = "Welcome to YourShop!!",
            ["ChangeEmailTitle"] = "YourShop Account Revisions Notice – E-mail Change",
            ["ChangePassword"] = "YourShop Account Revisions Notice – Password Change",
            ["MailTitleForgotPassword"] = "YourShop Account Revisions Notice – Reset Password",
            ["OrderMailSubject"] = "Thank you for your order, Order#",
            ["Subtotal"] = "Subtotal",
            ["NetSum"] = "Net Sum",
            ["Savings"] = "Total Savings",
            ["Tax"] = "Tax",
            ["Recycling"] = "Electronic Waste Recycling Fee",
            ["TotalPrice"] = "Total Price",
            ["AAFESSubject"] = "AAFES Parts Store Sku Quantity Notice",
            ["AAFESContent"] = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><title></title><style type='text/css'>* {font-family: Lucida Sans Unicode;font-size: 11px;}\t#maindiv {border: 3px double #808080;margin: auto;width: 500px;text-align: left;\t}</style></head><body><div id='maindiv'><div style='background-color: #E1F0FF; font-weight: bold; text-align: center; padding: 3px 0px 5px 0px; border-bottom: 1px solid #808080'>AAFES Parts Store Sku Quantity Notice</div><div style='padding: 8px; font-size: 13px'>{ResultPlace}</div><div style='background-color: #E9E9E9; font-size: 11px; text-align: left; padding: 3px 0px 5px 8px; border-top: 1px solid #808080'>Date: {CDate}, please check the inventory.</div></div></body></html>",
            ["AAFESItem"] = "<p>SkuID: {0}, Item Name: {1}, Current Quantity: {2}, is less then \"{3}\", in parts store.</p>",
            ["ShippingFee"] = "Shipping Fee",
            ["Product"] = "Product",
            ["Quantity"] = "Quantity",
            ["VorkasseUberweisung"] = "Vorkasse/Bank Transfer",
            ["Stripe"] = "Visa, Mastercard, etc.",
            ["SelfPick"] = "Cash payment upon pickup",
            ["SaveConfiguratorMailSubject"] = "YourShop Computer Configurator #",

         },
         ["de"] = new Dictionary<string, string>
         {
            ["WelcomeToYourShop"] = "Willkommen bei YourShop!",
            ["ChangeEmailTitle"] = "YourShop Konto Änderungen - E-Mail Adresse ändern",
            ["ChangePassword"] = "YourShop Konto Änderungen - Passwort ändern",
            ["MailTitleForgotPassword"] = "YourShop Konto Änderungen - Passwort zurücksetzen",
            ["OrderMailSubject"] = "Vielen Dank für Ihre Bestellung, #",
            ["Subtotal"] = "Zwischensumme",
            ["NetSum"] = "Nettosumme",
            ["Savings"] = "Sie Sparen insgesamt",
            ["Tax"] = "% MwSt",
            ["Recycling"] = "Elektroschrott Wiederverwertungs Gebühr",
            ["TotalPrice"] = "Gesamtpreis",
            ["ShippingFee"] = "Versand",
            ["Product"] = "Produkt",
            ["Quantity"] = "Menge",
            ["Stripe"] = "Visa, Mastercard und weitere",
            ["SelfPick"] = "Barzahlung bei Abholung",
            ["SaveConfiguratorMailSubject"] = "YourShop Computer Konfigurator #",
         }
      };
      public static string CurrencyFormat(this decimal Self)
      {
         var result = Self.ToString("C", CultureInfo.CreateSpecificCulture("de-DE"));

         return Self < 0 ? $"-{result}" : $"{result}";
      }

      // Method to get localized value
      public static string GetLocalizedValue(string cultureCode, string key)
      {
         if (Localizations.TryGetValue(cultureCode, out var cultureDict))
         {
            if (cultureDict.TryGetValue(key, out var value))
            {
               return value;
            }
            else
            {
               return "Key not found";
            }
         }
         else
         {
            return "Culture code not found";
         }
      }

      public static string GetLocalizedValueForPaymentMethod(string cultureCode, string key)
      {
         if (Localizations.TryGetValue(cultureCode, out var cultureDict))
         {
            if (cultureDict.TryGetValue(key, out var value))
            {
               return value;
            }
         }

         return key;
      }
   }
}
