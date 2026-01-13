import createErrorModal from "../../../exception/errorModal.js";

const createCategoryForm = document.getElementById("createCategoryForm");

const editCategoryForm = document.getElementById("editCategoryForm");

const nextCategoryPageBtn = document.getElementById("nextCategoryPageBtn");

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
      parentCategoryId:
        new FormData(event.target).get("parentCategoryId") == ""
          ? null
          : Number(new FormData(event.target).get("parentCategoryId")),
      isActive: new FormData(event.target).get("isActive") === "true",
    }),
  })
    .then((response) => {
      if (response.ok) {
        response.json().then(async () => {
          await refreshCategoriesList(createCategoryForm.dataset.page);
        });
        return;
      }
      throw new Error("Failed to create category");
    })
    .catch((error) => {
      console.error("Error creating category:", error);
      createErrorModal(
        "Error al crear categoría",
        "No se pudo crear la categoría. Por favor, intente de nuevo y si el problema persiste, contacte al administrador."
      );
    });
});

async function refreshCategoriesList(page = 1, nameFilter) {
  const categoryList = document.getElementById("categoriesList");
  await fetch(
    "/api/getCategories" +
      (nameFilter ? "?name=" + nameFilter : "?page=" + page),
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    }
  ).then((response) => {
    if (response.ok) {
      response.json().then((data) => {
        categoryList.innerHTML = "";
        data.data.forEach((category) => {
          const listItem = document.createElement("li");
          listItem.textContent = `${category.name}`;
          listItem.className = "list-group-item category-item";
          categoryList.appendChild(listItem);
        });
      });
    }
  });
}

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
