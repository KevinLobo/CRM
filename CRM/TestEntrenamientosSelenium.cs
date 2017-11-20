/*using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace CRM
{
    public class TestEntrenamientosSelenium
    {
        //[TestCase("01/01/2017 2:54 PM", "11/22/2017 2:55 PM")]
        public void RealizarBusqueda(String fechaInicio, String fechaFinal)
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                // codigo para conectar
                driver.Navigate().GoToUrl("http://localhost:51842/logIn.aspx");

                IWebElement usernameField = driver.FindElement(By.Id("userName"));
                usernameField.SendKeys("kevin3377");
                
                IWebElement passwordField = driver.FindElement(By.Id("password"));
                passwordField.SendKeys("kevin123");

                IWebElement submitButton = driver.FindElement(By.Id("logIn"));
                submitButton.Click();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.UrlToBe("http://localhost:51842/principal.aspx"));
                // fin de codigo de conexion


                driver.Navigate().GoToUrl("http://localhost:51842/entrenamientos.aspx");

                IWebElement fechaInicial = driver.FindElement(By.Id("fechaInicio"));
                fechaInicial.Clear();
                fechaInicial.SendKeys(fechaInicio);

                IWebElement fechaFin = driver.FindElement(By.Id("fechaFinal"));
                fechaFin.Clear();
                fechaFin.SendKeys(fechaFinal);

                IWebElement botonBuscar = driver.FindElement(By.Id("btnSubmit"));
                botonBuscar.Click();

                var wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait2.Until(ExpectedConditions.ElementExists((By.Id("GridViewEmpresa"))));

                IWebElement tablaResultados = driver.FindElement(By.Id("GridViewEmpresa"));

                Assert.AreEqual(true, tablaResultados.Enabled);
                Assert.AreEqual(true, tablaResultados.Displayed);
            }
        }
    }
}*/