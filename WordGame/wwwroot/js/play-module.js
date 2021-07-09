var Play_Module = (function () {
   function Init() {
      $("#play-button").on("click", startGame);
      hideElement($("#playground"));
      hideElement($("#loader-container"));
   }

   function startGame() {
      let $playButton = $(this);
      showElement($("#loader-container"));
      disableElement($playButton);
      loadContent();
   }

   function showElement($element) {
      $element.removeClass("hide");
   }

   function hideElement($element) {
      $element.addClass("hide");
   }

   function disableElement(element) {
      element.attr("disabled", true);
   }

   function enableElement(element) {
      element.attr("disabled", false);
   }

   function loadContent() {
      let inputWordValue = $("#input-word").val();
      let dictionaryCheck = $('#dictionary-check input[name=flexRadioDefault]:checked');
      let useCustomDictionary = false;
      if (dictionaryCheck.val() === "custom") {
         useCustomDictionary = true;
      }
      var playGameViewModel = {
         InputWordValue: inputWordValue,
         UseCustomDictionary: useCustomDictionary
      };
      requestServerData(playGameViewModel)
   }

   function requestServerData(object) {
      $.ajax({
         type: "POST",
         url: $("#main-container").data("homePlayGameUrl"),
         data: JSON.stringify(object),
         contentType: "application/json",
         success: loadResult,
         error: showError,
      });
   }

   function loadResult(data) {
      hideElement($("#loader-container"));
      enableElement($("#play-button"));
      $("#word-count").text(data.length);
      showElement($("#playground"));
      let contentContainer = $("#content");
      contentContainer.empty();
      $.each(data, function (index, value) {
         contentContainer.append($("<span>").text(value));
      });
   }

   function showError(XMLHttpRequest, textStatus, errorThrown) {
      console.log(errorThrown)
   }

   return {
      Init: function () {
         Init();
      },
   };
})();
