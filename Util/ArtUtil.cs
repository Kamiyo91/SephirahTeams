using System.Collections.Generic;
using System.IO;
using UI;
using UnityEngine;

namespace SephirahTeams.Util
{
    public static class ArtUtil
    {
        public static void GetArtWorks(DirectoryInfo dir)
        {
            try
            {
                if (dir.GetDirectories().Length != 0)
                {
                    var directories = dir.GetDirectories();
                    foreach (var t in directories) GetArtWorks(t);
                }

                foreach (var fileInfo in dir.GetFiles())
                {
                    var texture2D = new Texture2D(2, 2);
                    texture2D.LoadImage(File.ReadAllBytes(fileInfo.FullName));
                    var value = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height),
                        new Vector2(0f, 0f));
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                    ModParameters.ArtWorks[fileNameWithoutExtension] = value;
                }
            }
            catch
            {
                // ignored
            }
        }

        public static void SetBooksData(UISettingInvenEquipPageListSlot instance,
            List<BookModel> books, UIStoryKeyData storyKey)
        {
            if (ModParameters.PackageId != storyKey.workshopId || books.Count < 0) return;
            instance.img_IconGlow.enabled = true;
            instance.img_Icon.enabled = true;
            var icon = UISpriteDataManager.instance.GetStoryIcon("Chapter1");
            instance.img_IconGlow.sprite = icon.icon;
            instance.img_Icon.sprite = icon.icon;
            instance.txt_StoryName.text = "Sephirah Team";
        }

        public static void SetBooksData(UIInvenEquipPageListSlot instance,
            List<BookModel> books, UIStoryKeyData storyKey)
        {
            if (ModParameters.PackageId != storyKey.workshopId || books.Count < 0) return;
            instance.img_IconGlow.enabled = true;
            instance.img_Icon.enabled = true;
            var icon = UISpriteDataManager.instance.GetStoryIcon("Chapter1");
            instance.img_IconGlow.sprite = icon.icon;
            instance.img_Icon.sprite = icon.icon;
            instance.txt_StoryName.text = "Sephirah Team";
        }

        public static void GetThumbSprite(LorId bookId, ref Sprite result)
        {
            if (ModParameters.PackageId != bookId.packageId) return;
            if (!ModParameters.Sprites.TryGetValue(bookId.id, out var spriteName)) return;
            if (ModParameters.ArtWorks.TryGetValue(spriteName, out var spriteArt)) result = spriteArt;
        }

        public static void SetEpisodeSlots(UIBookStoryChapterSlot instance, UIBookStoryPanel panel,
            List<UIBookStoryEpisodeSlot> episodeSlots)
        {
            var uibookStoryEpisodeSlot = episodeSlots.Find(x =>
                x.books.Find(y => y.id.packageId == ModParameters.PackageId) != null);
            if (uibookStoryEpisodeSlot == null || instance.chapter != 7) return;
            var books = uibookStoryEpisodeSlot.books;
            var uibookStoryEpisodeSlot2 = episodeSlots[episodeSlots.Count - 1];
            books.RemoveAll(x => x.id.packageId == ModParameters.PackageId);
            uibookStoryEpisodeSlot2.Init(instance.chapter, books, instance);
        }
    }
}