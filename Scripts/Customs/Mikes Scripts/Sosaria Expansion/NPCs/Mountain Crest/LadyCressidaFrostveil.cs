using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SovereignOfTheShiverQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Sovereign of the Shiver"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Lady Cressida Frostveil*, noble of Mountain Crest, her breath curling like mist in the cold mountain air.\n\n" +
                    "Her cloak shimmers like frost, and she regards you with eyes as sharp as icicles.\n\n" +
                    "“My family’s crest bears the sigil of a forgotten king—the *Frostbound Sovereign*. Once ruler of the Ice Cavern depths, he forged minions from ice and ruled with a frozen heart. His roar still echoes, cracking stone and will alike.”\n\n" +
                    "“He has risen again, bound to the cavern’s cold curse. I am heir to his legacy—and to its undoing. But I am no warrior, and the Sovereign’s minions would end me before I could raise a blade.”\n\n" +
                    "**Slay the Frostbound Sovereign** and bring peace to my bloodline and these mountains. His reign must end, once and for all.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the mountain shall remain cursed, and my name chained to his legacy of frost and fear.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Sovereign still reigns? I feel the chill deepen. His roar haunts me at night, shaking the very stones of Mountain Crest.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It is done? His roar is silenced?\n\n" +
                       "You have not only freed me from my ancestral shame, but you’ve unbound this land from the cold grip of the past.\n\n" +
                       "Take this cap—worn by the free spirits of the mountains. May it remind you of the song of the wind and the warmth of freedom.";
            }
        }

        public SovereignOfTheShiverQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FrostboundSovereign), "Frostbound Sovereign", 1));
            AddReward(new BaseReward(typeof(StreetPerformersCap), 1, "StreetPerformer’s Cap"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Sovereign of the Shiver'!");
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

    public class LadyCressidaFrostveil : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SovereignOfTheShiverQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBowyer());
        }

        [Constructable]
        public LadyCressidaFrostveil()
            : base("the Noble of Mountain Crest", "Lady Cressida Frostveil")
        {
        }

        public LadyCressidaFrostveil(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 80, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale, frosted skin tone
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Icy silver
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1152, Name = "Frostweave Blouse" }); // Pale icy blue
            AddItem(new FancyKilt() { Hue = 1153, Name = "Glacier Silk Skirt" });
            AddItem(new Cloak() { Hue = 1151, Name = "Frostveil Mantle" }); // Frosted white cloak
            AddItem(new FeatheredHat() { Hue = 1153, Name = "Crested Noble’s Cap" });
            AddItem(new Sandals() { Hue = 1152, Name = "Icebound Sandals" });

            Backpack backpack = new Backpack();
            backpack.Hue = 0x48E; // Cool grey-blue
            backpack.Name = "Frostveil Satchel";
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
