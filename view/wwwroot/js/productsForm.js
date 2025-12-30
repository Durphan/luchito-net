const createProductForm = document.getElementById("createProductForm");

const editProductForm = document.getElementById("editProductForm");

const deleteProductForm = document.getElementById("deleteProductForm");

createProductForm?.addEventListener("submit", async (event) => {
  event.preventDefault();
  await fetch("/api/createProduct", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      name: new FormData(event.target).get("name"),
      categoryId: Number(new FormData(event.target).get("categoryId")),
    }),
  }).then((response) => {
    if (response.ok) {
      alert("Producto creado exitosamente");
    } else {
      alert("Error al crear el producto");
    }
  });
});

editProductForm?.addEventListener("submit", async (event) => {
  event.preventDefault();
  await fetch("/api/updateProduct/" + new FormData(event.target).get("id"), {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      name: new FormData(event.target).get("name"),
      categoryId: Number(new FormData(event.target).get("categoryId")),
    }),
  }).then((response) => {
    if (response.ok) {
      alert("Producto editado exitosamente");
    } else {
      alert("Error al editar el producto");
    }
  });
});

deleteProductForm?.addEventListener("submit", async (event) => {
  event.preventDefault();
  await fetch("/api/deleteProduct/" + new FormData(event.target).get("id"), {
    method: "DELETE",
  }).then((response) => {
    if (response.ok) {
      alert("Producto eliminado exitosamente");
    } else {
      alert("Error al eliminar el producto");
    }
  });
});
