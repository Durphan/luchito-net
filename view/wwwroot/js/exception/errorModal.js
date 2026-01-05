export default function createErrorModal(title, message) {
  $("#createErrorModalLabel").text(title);
  $("#createErrorMessage").text(message);
  $("#createErrorModal").modal("show");
}
