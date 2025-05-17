using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HellhoundsHellQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Hellhound’s Hell"; } }

        public override object Description
        {
            get
            {
                return
                    "*Garrick Flameward*—his crimson robes flicker as though embers dance across them, eyes smoldering with barely-contained fury.\n\n" +
                    "“The Stygian Hellhound—its breath is fire, but not like mine. Mine warms, guides. His burns, devours, mocks all that I’ve built!”\n\n" +
                    "“I’ve walked the edge of volcanoes, mastered the Ember Tongue... but that creature *dims* my wards. Its aura... *laughs* at flame.”\n\n" +
                    "“I’ve crafted ember talismans to shield you—temporary, fragile. But it will give you a chance to fight back. Go to Exodus Dungeon. Hunt the beast. Burn the Hellhound, before it burns *me* from within.”\n\n" +
                    "**Slay the Stygian Hellhound** and return before the embers cool.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then you’ll leave me to smolder in the shadows? Fine. But beware—the beast’s breath might find *you* next.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still alive, are you? Then so is *he*. The Hellhound’s fire still mocks me. Go, before it feeds again.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it. I feel it—the air is lighter, the flames no longer twist in torment.\n\n" +
                       "*Take this: the GraniteHammer.* Not just a tool, but a weapon of earth and fire, tempered against infernal wrath.\n\n" +
                       "May you wield it with the strength that tamed hell’s own hound.";
            }
        }

        public HellhoundsHellQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(StygianHellhound), "Stygian Hellhound", 1));
            AddReward(new BaseReward(typeof(GraniteHammer), 1, "GraniteHammer"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Hellhound’s Hell'!");
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

    public class GarrickFlameward : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(HellhoundsHellQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage());
        }

        [Constructable]
        public GarrickFlameward()
            : base("the Pyromancer", "Garrick Flameward")
        {
        }

        public GarrickFlameward(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 100, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Ashen tan
            HairItemID = 0x203C; // Long Hair
            HairHue = 1359; // Fiery Red
            FacialHairItemID = 0x204B; // Long Beard
            FacialHairHue = 1359; // Same fiery red
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1358, Name = "Flameweave Robe" }); // Bright Ember Red
            AddItem(new BodySash() { Hue = 1357, Name = "Talisman of Embers" });
            AddItem(new WizardsHat() { Hue = 1154, Name = "Cinderpoint Hat" }); // Deep Charcoal
            AddItem(new Sandals() { Hue = 1109, Name = "Ashen Sandals" }); // Dark Grey
            AddItem(new GnarledStaff() { Hue = 1360, Name = "Firecaller’s Crook" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1153; // Smoldering Black
            backpack.Name = "Pyromancer’s Satchel";
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
