using UnityEngine;

namespace adefagia.Graph
{
    public class GridStatus : MonoBehaviour
    {
        
        public Material occupiedMaterial;
        public Material selectMaterial;
        public Material hoverMaterial;

        private Material _defaultMaterial;
        private MeshRenderer _meshRenderer;

        public Grid Grid { get; set; }

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _defaultMaterial = _meshRenderer.material;
        }

        void Update()
        {
            if (Grid.Occupied)
            {
                ChangeMaterial(occupiedMaterial);
            }
            else if (Grid.IsSelect)
            {
                ChangeMaterial(selectMaterial);
            }
            else if (Grid.IsHover)
            {
                ChangeMaterial(hoverMaterial);
            }
            else
            {
                ResetMaterial();
            }

        }
        
        private void ChangeMaterial(Material material)
        {
            _meshRenderer.material = material;
        }
        
        private void ResetMaterial()
        {
            _meshRenderer.material = _defaultMaterial;
        }
    }
}
