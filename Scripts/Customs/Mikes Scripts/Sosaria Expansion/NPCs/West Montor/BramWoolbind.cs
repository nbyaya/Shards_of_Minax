using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DouseTheBlazingLlamaQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Douse the BlazingLlama"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Bram Woolbind*, the esteemed weaver of West Montor, hunched over a scorched bolt of fabric.\n\n" +
                    "His hands tremble—not with age, but fury—as he gestures to charred tapestries and smoldering spools.\n\n" +
                    "\"This was to be my masterpiece! A grand display of our harvest pride, spun from the finest llama wool... until that *cursed beast* scorched every thread!\"\n\n" +
                    "\"They spoke of flame-spun creatures along the old Silk Roads, but never did I think one would haunt my trade! It dwells near the **Gate of Hell**, that blazing llama—a mockery of my craft!\"\n\n" +
                    "\"*Douse it*, adventurer. Slay the **BlazingLlama**, lest its fire-spun wool ignites every loom in Sosaria!\"\n\n" +
                    "\"Bring me peace, and I’ll reward you with something peculiar—a **CupOfSlime**, a relic from my travels, odd in taste but priceless to some.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then watch the flames grow, traveler. My looms may fall, but the fire will find more to consume.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Is it still out there? I can smell the scorched wool in every breeze...";
            }
        }

        public override object Complete
        {
            get
            {
                return "Bless the threads and curse the flame, you've done it!\n\n" +
                       "No longer will that fiery fiend torch my craft! Here, take this—**CupOfSlime**. A strange brew from distant lands, but a token of my gratitude.\n\n" +
                       "May your paths stay cool, and your cloak never catch fire.";
            }
        }

        public DouseTheBlazingLlamaQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BlazingLlama), "BlazingLlama", 1));
            AddReward(new BaseReward(typeof(CupOfSlime), 1, "CupOfSlime"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Douse the BlazingLlama'!");
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

    public class BramWoolbind : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DouseTheBlazingLlamaQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWeaver());
        }

        [Constructable]
        public BramWoolbind()
            : base("the Town Weaver", "Bram Woolbind")
        {
        }

        public BramWoolbind(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Slightly sun-weathered tone
            HairItemID = 0x203B; // Short hair
            HairHue = 1154; // Fiery red
            FacialHairItemID = 0x204B; // Full beard
            FacialHairHue = 1154;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2075, Name = "Emberthread Shirt" });
            AddItem(new LongPants() { Hue = 2419, Name = "Charcoal Loom Pants" });
            AddItem(new HalfApron() { Hue = 2201, Name = "Weaver's Flamebound Apron" });
            AddItem(new Sandals() { Hue = 2403, Name = "Ashen Weaving Sandals" });
            AddItem(new FeatheredHat() { Hue = 1157, Name = "Scarlet Silkhunter Hat" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1161;
            backpack.Name = "Spindlebag";
            AddItem(backpack);

            AddItem(new SewingNeedle() { Hue = 1166, Name = "Traveler's Needle" });
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
