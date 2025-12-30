const createCategoryForm = document.getElementById("createCategoryForm");

const editCategoryForm = document.getElementById("editCategoryForm");

const deleteCategoryForm = document.getElementById("deleteCategoryForm");

createCategoryForm?.addEventListener("submit", async (event) => {
  event.preventDefault();
  await fetch("/api/createCategory", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      name: String(new FormData(event.target).get("name")),
      parentCategoryId: Number(
        new FormData(event.target).get("parentCategoryId")
      ),
      isActive: new FormData(event.target).get("isActive") === "true",
    }),
  }).then((response) => {
    if (response.ok) {
      alert("Categoria creada exitosamente");
    }
  });
});

editCategoryForm?.addEventListener("submit", async (event) => {
  event.preventDefault();
  await fetch("/api/updateCategory/" + new FormData(event.target).get("id"), {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      name: String(new FormData(event.target).get("name")),
      parentCategoryId: Number(
        new FormData(event.target).get("parentCategoryId")
      ),
      isActive: new FormData(event.target).get("isActive") === "true",
    }),
  }).then((response) => {
    if (response.ok) {
      alert("Categoria editada exitosamente");
    }
  });
});

deleteCategoryForm?.addEventListener("submit", async (event) => {
  event.preventDefault();
  await fetch("/api/deleteCategory/" + new FormData(event.target).get("id"), {
    method: "DELETE",
  }).then((response) => {
    if (response.ok) {
      alert("Categoria eliminada exitosamente");
    }
  });
});
