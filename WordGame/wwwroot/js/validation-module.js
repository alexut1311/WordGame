var Validation_Module = (function () {
   function Init() {
      validateDictionaryInput();
      validatePlayInput();
   }

   function validateDictionaryInput() {
      let dictionaryTextarea = $("#add-new-words");
      if (dictionaryTextarea) {
         dictionaryTextarea.bind('keypress', onlyLettersAndSpaceInput);
      }
   }

   function validatePlayInput() {
      let playInput = $("#input-word");
      if (playInput) {
         playInput.bind('keypress', onlyLettersInput);
      }
   }

   function onlyLettersInput(event) {
      return verifyInput(event, /^[A-Za-z]+$/, $("#error-for-play-input"));
   }

   function onlyLettersAndSpaceInput(event) {
      return verifyInput(event, /^[A-Za-z ]+$/, $("#error-for-add-new-words"));
   }

   function verifyInput(event, regexRule, $errorElement) {
      var value = String.fromCharCode(event.which);
      var pattern = new RegExp(regexRule);
      if (!pattern.test(value)) {
         $errorElement.css({
            "display": "block",
            "color": "red"
         })
      } else {
         $errorElement.css("display", "none")
      }
      return pattern.test(value);
   }

   return {
      Init: function () {
         Init();
      },
   };
})();
