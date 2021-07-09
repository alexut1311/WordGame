using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using NUnit.Framework;
using WordGame.BL.Interfaces;
using WordGame.Controllers;
using WordGame.Models;
using WordGame.Tests.Helpers;

namespace WordGame.Tests.ControllerTests
{
   internal class HomeControllerTests
   {
      private HomeController _homeController;
      private Mock<IDictionaryLogic> _dictionaryLogic;

      [SetUp]
      public void Setup()
      {
         _dictionaryLogic = new Mock<IDictionaryLogic>();
         _homeController = new HomeController(_dictionaryLogic.Object);
      }

      [Test]
      public void Index_View_Test()
      {
         ViewResult result = _homeController.Index() as ViewResult;
         Assert.That(result, Is.Not.Null);
         Assert.That(result.ViewName, Is.EqualTo("Index"));
      }

      [Test]
      public void Dictionary_Get_Test()
      {
         _dictionaryLogic.Setup(x => x.GetWordsFromDictionary(It.IsAny<string>())).Returns(ValueHelpers.GetWords()).Verifiable();

         ViewResult result = _homeController.Dictionary() as ViewResult;
         Assert.That(result, Is.Not.Null);
         Assert.That(result.Model, Is.Not.Null);
         Assert.That(result.ViewName, Is.EqualTo("Dictionary"));
         DictionaryViewModel model = result.Model as DictionaryViewModel;
         Assert.That(model.CurrentWords, Is.Not.Empty);
         Assert.That(model.CurrentWords.Count, Is.EqualTo(3));

         _dictionaryLogic.Verify(x => x.GetWordsFromDictionary(It.IsAny<string>()), Times.Exactly(1));
      }

      [Test]
      public void Dictionary_Post_Test()
      {
         _dictionaryLogic.Setup(x => x.AddWordsFromDictionary(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

         RedirectToActionResult result = _homeController.Dictionary(new DictionaryViewModel()) as RedirectToActionResult;
         Assert.That(result, Is.Not.Null);
         Assert.That(result.ActionName, Is.EqualTo("Dictionary"));
         Assert.That(result.ControllerName, Is.EqualTo("Home"));

         _dictionaryLogic.Verify(x => x.AddWordsFromDictionary(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
      }

      [Test]
      public void PlayGame_Test()
      {
         _dictionaryLogic.Setup(x => x.GenerateWordPermutations(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(ValueHelpers.GetWords()).Verifiable();

         var result = _homeController.PlayGame(new PlayViewModel()) as JsonResult;
         Assert.That(result, Is.Not.Null);
         Assert.That(result.Value, Is.Not.Null);

         _dictionaryLogic.Verify(x => x.GenerateWordPermutations(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Exactly(1));
      }
   }
}
