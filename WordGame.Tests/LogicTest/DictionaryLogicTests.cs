using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordGame.BL.Classes;
using WordGame.BL.Interfaces;
using WordGame.Tests.Helpers;

namespace WordGame.Tests.LogicTest
{
   internal class DictionaryLogicTests
   {
      private IDictionaryLogic _dictionaryLogic;
      private Mock<IFileLogic> _fileLogic;

      [SetUp]
      public void Setup()
      {
         _fileLogic = new Mock<IFileLogic>();
         _dictionaryLogic = new DictionaryLogic(_fileLogic.Object);
      }

      [Test]
      public void GetWordsFromDictionary_Test()
      {
         _fileLogic.Setup(x => x.ReadWordsFromFile(It.IsAny<string>())).Returns(ValueHelpers.GetWords()).Verifiable();

         var result = _dictionaryLogic.GetWordsFromDictionary(It.IsAny<string>());
         Assert.That(result, Is.Not.Null);
         Assert.That(result, Is.Not.Empty);
         Assert.That(result.Count, Is.EqualTo(3));

         _fileLogic.Verify(x => x.ReadWordsFromFile(It.IsAny<string>()), Times.Exactly(1));
      }

      [Test]
      public void AddWordsFromDictionary_Test()
      {
         _fileLogic.Setup(x => x.WriteWordsToFile(It.IsAny<string>(), It.IsAny<IList<string>>())).Verifiable();

         _dictionaryLogic.AddWordsFromDictionary(It.IsAny<string>(), ValueHelpers.GetStringToSplit());

         _fileLogic.Verify(x => x.WriteWordsToFile(It.IsAny<string>(), It.IsAny<IList<string>>()), Times.Exactly(1));
      }

      [Test]
      public void GenerateWordPermutations_CustomDictionary_Test()
      {
         _fileLogic.Setup(x => x.ReadWordsFromFile(It.IsAny<string>())).Returns(ValueHelpers.GetDictionaryWords()).Verifiable();

         var result = _dictionaryLogic.GenerateWordPermutations("the", It.IsAny<string>(), true);

         Assert.That(result, Is.Not.Null);
         Assert.That(result, Is.Not.Empty);
         Assert.That(result.Count, Is.GreaterThanOrEqualTo(6));

         _fileLogic.Verify(x => x.ReadWordsFromFile(It.IsAny<string>()), Times.Exactly(1));
      }
   }
}
