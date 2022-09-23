mergeInto(LibraryManager.library, {
  JavaScriptAlert: function (str) {
    window.alert(UTF8ToString(str));
  },
});