const API_URL = "http://localhost:5000/api/pokemon";

function getSpriteUrl(id) {
  return `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/${id}.png`;
}

let currentPokemon = null;

// ── Drawer ──
const drawer        = document.getElementById("add-drawer");
const drawerOverlay = document.getElementById("drawer-overlay");

document.getElementById("open-drawer-btn").addEventListener("click", () => {
  drawer.classList.add("open");
  drawerOverlay.classList.add("open");
});

function closeDrawer() {
  drawer.classList.remove("open");
  drawerOverlay.classList.remove("open");
}

document.getElementById("drawer-close").addEventListener("click", closeDrawer);
drawerOverlay.addEventListener("click", closeDrawer);

// ── Type chip toggle (Add form) ──
document.querySelectorAll("#add-drawer .type-chip").forEach(chip => {
  chip.addEventListener("click", () => {
    document.querySelectorAll("#add-drawer .type-chip").forEach(c => c.classList.remove("active"));
    chip.classList.add("active");
    document.getElementById("type").value = chip.dataset.type;
  });
});

// ── Type chip toggle (Edit form) ──
document.querySelectorAll("#edit-type-row .type-chip").forEach(chip => {
  chip.addEventListener("click", () => {
    document.querySelectorAll("#edit-type-row .type-chip").forEach(c => c.classList.remove("active"));
    chip.classList.add("active");
    document.getElementById("edit-type").value = chip.dataset.type;
  });
});

// ── Load & display Pokémon ──
async function loadPokemons() {
  const response = await fetch(API_URL);
  const pokemons = await response.json();

  const grid = document.getElementById("card-grid");
  grid.innerHTML = "";

  pokemons.forEach(pokemon => {
    const card = document.createElement("div");
    card.className = "pokedex-box";
    card.innerHTML = `
      <div class="card-id">#${pokemon.id}</div>
      <div class="card-name">${pokemon.name}</div>
      <img src="${getSpriteUrl(pokemon.id)}" alt="${pokemon.name}" class="sprite">
      <p><span class="type-badge type-${pokemon.type}">${pokemon.type}</span></p>
    `;
    card.addEventListener("click", () => openModal(pokemon));
    grid.appendChild(card);
  });
}

// ── Open detail modal ──
function openModal(pokemon) {
  currentPokemon = pokemon;
  showViewMode(pokemon);
  document.getElementById("pokemon-modal").classList.add("open");
}

function showViewMode(pokemon) {
  document.getElementById("view-mode").style.display = "block";
  document.getElementById("edit-mode").style.display  = "none";

  document.getElementById("modal-info").innerHTML = `
    <h2>${pokemon.name}</h2>
    <img src="${getSpriteUrl(pokemon.id)}" alt="${pokemon.name}" class="sprite"
         style="width:120px;height:120px;margin:12px auto;display:block;">
    <p><span class="type-badge type-${pokemon.type}">${pokemon.type}</span></p>
    <p><strong>Height:</strong> ${pokemon.height} m &nbsp;|&nbsp; <strong>Weight:</strong> ${pokemon.weight} kg</p>
    <p style="margin-top:10px;">${pokemon.description}</p>
  `;
}

// ── Close modal ──
function closeModal() {
  document.getElementById("pokemon-modal").classList.remove("open");
  currentPokemon = null;
}

document.getElementById("modal-close").addEventListener("click", closeModal);

window.addEventListener("click", (e) => {
  const modal = document.getElementById("pokemon-modal");
  if (e.target === modal) closeModal();
});

// ── Edit button → switch to edit mode ──
document.getElementById("btn-edit").addEventListener("click", () => {
  if (!currentPokemon) return;

  document.getElementById("edit-name").value        = currentPokemon.name;
  document.getElementById("edit-height").value      = currentPokemon.height;
  document.getElementById("edit-weight").value      = currentPokemon.weight;
  document.getElementById("edit-description").value = currentPokemon.description;
  document.getElementById("edit-type").value        = currentPokemon.type;

  // Highlight the current type chip
  document.querySelectorAll("#edit-type-row .type-chip").forEach(c => {
    c.classList.toggle("active", c.dataset.type === currentPokemon.type);
  });

  document.getElementById("view-mode").style.display = "none";
  document.getElementById("edit-mode").style.display  = "block";
});

// ── Cancel edit ──
document.getElementById("btn-cancel").addEventListener("click", () => {
  showViewMode(currentPokemon);
});

// ── TOAST ──
let toastTimer;
function showToast(msg, type = 'info') {
  const toast = document.getElementById('toast');
  const icons = { success: '✅', error: '❌', info: 'ℹ️' };
  document.getElementById('toastMsg').textContent = msg;
  document.getElementById('toastIcon').textContent = icons[type] || 'ℹ️';
  toast.className = `toast ${type}`;
  clearTimeout(toastTimer);
  void toast.offsetWidth;
  toast.classList.add('show');
  toastTimer = setTimeout(() => toast.classList.remove('show'), 3000);
}

// ── CONFIRM DIALOG ──
let confirmCallback = null;
function showConfirm(name, onConfirm) {
  document.getElementById('confirmName').textContent = name;
  confirmCallback = onConfirm;
  document.getElementById('confirmOverlay').classList.add('open');
}
document.getElementById('confirmYes').addEventListener('click', () => {
  document.getElementById('confirmOverlay').classList.remove('open');
  if (confirmCallback) confirmCallback();
  confirmCallback = null;
});
document.getElementById('confirmNo').addEventListener('click', () => {
  document.getElementById('confirmOverlay').classList.remove('open');
  confirmCallback = null;
});

// ── DELETE ──
document.getElementById("btn-delete").addEventListener("click", async () => {
  if (!currentPokemon) return;

  showConfirm(currentPokemon.name, async () => {
    try {
      const response = await fetch(`${API_URL}/${currentPokemon.id}`, { method: "DELETE" });
      if (response.ok) {
        closeModal();
        loadPokemons();
        showToast(`${currentPokemon.name} deleted!`, 'success');
      } else {
        showToast('Failed to delete Pokémon.', 'error');
      }
    } catch {
      showToast('Could not reach server.', 'error');
    }
  });
});

// ── Save edits ──
document.getElementById("edit-form").addEventListener("submit", async (e) => {
  e.preventDefault();

  const type = document.getElementById("edit-type").value;
  if (!type) {
    alert("Please select a type.");
    return;
  }

  const updated = {
    name:          document.getElementById("edit-name").value,
    type:          type,
    height:        parseFloat(document.getElementById("edit-height").value),
    weight:        parseFloat(document.getElementById("edit-weight").value),
    description:   document.getElementById("edit-description").value,
    isEncountered: true
  };

  const response = await fetch(`${API_URL}/${currentPokemon.id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(updated)
  });

  if (response.ok) {
    const saved = await response.json();
    currentPokemon = saved;
    showViewMode(saved);
    loadPokemons();
  } else {
    alert("Failed to update Pokémon.");
  }
});

// ── Add new Pokémon ──
document.getElementById("pokemon-form").addEventListener("submit", async (e) => {
  e.preventDefault();

  const type = document.getElementById("type").value;
  if (!type) {
    alert("Please select a type by clicking one of the type chips.");
    return;
  }

  const newPokemon = {
    name:          document.getElementById("name").value,
    type:          type,
    height:        parseFloat(document.getElementById("height").value),
    weight:        parseFloat(document.getElementById("weight").value),
    description:   document.getElementById("description").value,
    isEncountered: true
  };

  const response = await fetch(API_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(newPokemon)
  });

  if (response.ok) {
    alert("Pokémon added successfully!");
    e.target.reset();
    document.querySelectorAll("#add-drawer .type-chip").forEach(c => c.classList.remove("active"));
    document.getElementById("type").value = "";
    closeDrawer();
    loadPokemons();
  } else {
    alert("Failed to add Pokémon.");
  }
});

loadPokemons();