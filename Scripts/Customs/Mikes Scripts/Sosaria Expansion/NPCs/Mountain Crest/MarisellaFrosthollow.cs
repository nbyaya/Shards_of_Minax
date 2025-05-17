using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SongOfTheSirensChill : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Song of the Siren’s Chill"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Marisella Frosthollow*, the ethereal bard of Mountain Crest.\n\n" +
                    "Her fingers rest lightly on a frost-touched harp, eyes distant, yet sharp with warning.\n\n" +
                    "“Few heed my songs, traveler. They think me mad, or drunk on sorrow. But the Ice Cavern sings too, and not with a friendly tune.”\n\n" +
                    "“A creature dwells there, a *Frosted Seductress*, whose voice lures the brave and foolish alike into a frozen sleep… forever.”\n\n" +
                    "“I’ve written of her in verses, warned of her in rhyme. But now, I must ask for action.”\n\n" +
                    "**Slay the Frosted Seductress** and silence her song—for only then will Mountain Crest rest easy.”\n\n" +
                    "*Be warned: her voice twists magic. Silence all musical enchantments before you strike.*";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may your ears stay sharp and your heart stay cold—for her song will claim more souls before it stills.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still her song echoes through the cavern? My strings tremble with her tune, even now. Please, end her chill melody.";
            }
        }

        public override object Complete
        {
            get
            {
                return "Her song is no more? Truly?\n\n" +
                       "Then the frost may thaw, and my music can return to lighter airs.\n\n" +
                       "Take this, *IcicleStaff*, forged in her defeat. Let it play a new melody—one of strength, not sorrow.";
            }
        }

        public SongOfTheSirensChill() : base()
        {
            AddObjective(new SlayObjective(typeof(FrostedSeductress), "Frosted Seductress", 1));
            AddReward(new BaseReward(typeof(IcicleStaff), 1, "IcicleStaff"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Song of the Siren’s Chill'!");
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

    public class MarisellaFrosthollow : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SongOfTheSirensChill) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBaker());
        }

        [Constructable]
        public MarisellaFrosthollow()
            : base("the Frost-Touched Bard", "Marisella Frosthollow")
        {
        }

        public MarisellaFrosthollow(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 85, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1152; // Pale, frosty skin tone
            HairItemID = 0x2049; // Long hair
            HairHue = 1153; // Frost white
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1150, Name = "Glacial Gown" }); // Icy blue hue
            AddItem(new Cloak() { Hue = 1153, Name = "Chillwind Cloak" }); // White frost-like hue
            AddItem(new FeatheredHat() { Hue = 1150, Name = "Bard’s Frostcap" });
            AddItem(new ThighBoots() { Hue = 1151, Name = "Winterstride Boots" });

            AddItem(new ResonantHarp() { Hue = 1152, Name = "Frosthollow’s Lament" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bard's Satchel";
            AddItem(backpack);
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
