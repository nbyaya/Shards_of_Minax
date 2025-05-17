using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class VoicesOfTheWoodQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Voices of the Wood"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Nyssa Glasswater*, the delicate artisan of Fawn’s famed crystal flutes, standing beside a workbench strewn with shattered glass.\n\n" +
                    "Her fingers tremble as she holds a flute splintered by unseen forces, her eyes reflecting the light of a nearby kiln.\n\n" +
                    "“It sings… but not for us.”\n\n" +
                    "“In the forest’s heart, the **Echofern** calls. Its voice—a foul mimicry—corrupts all that listens. My flutes, once tuned to the wind, now shatter at its song. The plants twist, roots rise, and my art dies with each note.”\n\n" +
                    "“I’ve salvaged a fragment of its bark. It hums, even in stillness. This is no natural growth.”\n\n" +
                    "**Destroy the Echofern** before it spreads further, or Fawn may no longer know peace or beauty.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I will remain, surrounded by shards of what once was. May the Echofern not find its way to your dreams.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it sings? I hear its echoes in the night, louder than ever.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The forest is quiet again?\n\n" +
                       "Then take these—*HarmonyGauntlets.* I forged them from the same glass as my flutes, tempered now with silence and strength.\n\n" +
                       "May your hands craft peace where mine have failed.";
            }
        }

        public VoicesOfTheWoodQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Echofern), "Echofern", 1));
            AddReward(new BaseReward(typeof(HarmonyGauntlets), 1, "HarmonyGauntlets"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Voices of the Wood'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class NyssaGlasswater : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(VoicesOfTheWoodQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGlassblower());
        }

        [Constructable]
        public NyssaGlasswater()
            : base("the Crystal Artisan", "Nyssa Glasswater")
        {
        }

        public NyssaGlasswater(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 70, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 33770; // Pale complexion
            HairItemID = 0x203C; // Long Hair
            HairHue = 1153; // Ice-blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1153, Name = "Glassweaver's Gown" }); // Pale blue, crystalline shimmer
            AddItem(new Cloak() { Hue = 2101, Name = "Mistveil Cloak" }); // Soft grey, flowing
            AddItem(new Sandals() { Hue = 2105, Name = "Kiln-worn Sandals" }); // Ashen leather
            AddItem(new FlowerGarland() { Hue = 1150, Name = "Crystal Bloom Wreath" }); // Decorative headpiece

            AddItem(new ArtificerWand() { Hue = 1153, Name = "Resonant Rod" }); // For delicate glass shaping
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
