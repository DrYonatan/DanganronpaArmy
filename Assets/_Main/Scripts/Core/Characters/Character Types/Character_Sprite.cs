using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CHARACTERS
{
    public class Character_Sprite : Character
    {
        private const string SPRITE_RENDERED_PARENT_NAME = "Renderers";
        private CanvasGroup rootCG => root.GetComponent<CanvasGroup>();

        private List<CharacterSpriteLayer> layers = new List<CharacterSpriteLayer>();

        private string artAssetsDirectory = "";

        public override bool isVisible => isRevealing || rootCG.alpha == 1;

        public Character_Sprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab)
        {
            rootCG.alpha = ENABLE_ON_START ? 1 : 0;
            artAssetsDirectory = rootAssetsFolder + "/Images";
            GetLayers();
            Debug.Log($"Created Sprite Character '{name}'");
        }

        private void GetLayers()
        {
            Transform rendererRoot = animator.transform.Find(SPRITE_RENDERED_PARENT_NAME);

            if (rendererRoot == null)
                return;

            for(int i = 0; i <rendererRoot.transform.childCount; i++)
            {
                Transform child = rendererRoot.transform.GetChild(i);

                Image rendererImage = child.GetComponent<Image>();

                if(rendererImage != null)
                {
                    CharacterSpriteLayer layer = new CharacterSpriteLayer(rendererImage, i);
                    layers.Add(layer);
                    child.name = $"Later: {i}";
                }
            }
        }

        public void SetSprite(Sprite sprite, int layer = 0)
        {
            layers[layer].SetSprite(sprite);
        }

        public Sprite GetSprite(string spriteName)
        {
            return Resources.Load<Sprite>($"{artAssetsDirectory}/{spriteName}");
        }

        public Coroutine TransitionSprite(Sprite sprite, int layer = 0, float speed = 1)
        {
            CharacterSpriteLayer spriteLayer = layers[layer];

            return spriteLayer.TransitionSprite(sprite, speed);
        }
        public override IEnumerator ShowingOrHiding(bool show)
        {
            float targetAlpha = show ? 1f : 0;
            CanvasGroup self = rootCG;


            while (self.alpha != targetAlpha)
            {
                self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, 3f * Time.deltaTime);
                yield return null;
            }

            co_revealing = null;
            co_hiding = null;
        }
    }
}