const createOrderForm = document.getElementById("createOrderForm");

const editOrderForm = document.getElementById("editOrderForm");

const deleteOrderForm = document.getElementById("deleteOrderForm");

createOrderForm?.addEventListener("submit", async (event) => {
  event.preventDefault();
  await fetch("/api/createOrder", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      productId: Number(new FormData(event.target).get("productId")),
      quantity: Number(new FormData(event.target).get("quantity")),
      stateId: Number(new FormData(event.target).get("stateId")),
      isBoxed: new FormData(event.target).get("boxed") === "on",
    }),
  }).then((response) => {
    if (response.ok) {
      alert("Orden creada exitosamente");
    } else {
      alert("Error al crear la orden");
    }
  });
});

editOrderForm?.addEventListener("submit", async (event) => {
  event.preventDefault();
  await fetch("/api/updateOrder/" + new FormData(event.target).get("id"), {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      productId: Number(new FormData(event.target).get("productId")),
      quantity: Number(new FormData(event.target).get("quantity")),
      stateId: Number(new FormData(event.target).get("stateId")),
      isBoxed: new FormData(event.target).get("boxed") === "on" ?? false,
    }),
  }).then((response) => {
    if (response.ok) {
      alert("Orden actualizada exitosamente");
    } else {
      alert("Error al actualizar la orden");
    }
  });
});

deleteOrderForm?.addEventListener("submit", async (event) => {
  event.preventDefault();
  await fetch("/api/deleteOrder/" + new FormData(event.target).get("id"), {
    method: "DELETE",
  }).then((response) => {
    if (response.ok) {
      alert("Orden eliminada exitosamente");
    } else {
      alert("Error al eliminar la orden");
    }
  });
});
