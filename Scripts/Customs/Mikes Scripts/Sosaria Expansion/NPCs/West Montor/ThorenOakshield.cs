using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ToppleTheBlazingTitanQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Topple the BlazingTitan"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Thoren Oakshield*, Captain of the West Montor Militia.\n\n" +
                    "His armor is scorched in places, the breastplate bearing a faded emblem of a tree engulfed in flame. A deep scar runs from his brow to his cheek, a silent testament to battles past.\n\n" +
                    "“The *BlazingTitan* marches again. I can feel the heat on the wind, see the flames flicker behind closed eyes. Years ago, we drove it back, but it was not slain. It smashed our barricades, seared our land—and left this scar.”\n\n" +
                    "“It comes now for West Montor. The Gate of Hell cannot contain it much longer. If we don't act, the Titan will raze everything. Our homes, our fields... gone.”\n\n" +
                    "“Will you stand with us? Will you face the fire and bring down this beast?”\n\n" +
                    "**Slay the BlazingTitan** before it breaks free and consumes the town.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the barricades hold. But know this: fire does not wait, and fear will not quench it.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still lives? The heat grows, and the townsfolk stir in fear. We cannot hold much longer.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You did it... the Titan falls, and with it, the flames recede.\n\n" +
                       "You’ve saved West Montor from the same fate that nearly claimed us before. Take this **UncrackedGeode**, pulled from the Titan’s lair—it holds secrets yet untouched by fire.";
            }
        }

        public ToppleTheBlazingTitanQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BlazingTitan), "BlazingTitan", 1));
            AddReward(new BaseReward(typeof(UncrackedGeode), 1, "UncrackedGeode"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Topple the BlazingTitan'!");
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

    public class ThorenOakshield : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ToppleTheBlazingTitanQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSwordWeapon()); // As Captain of the Militia, weapon dealing fits.
        }

        [Constructable]
        public ThorenOakshield()
            : base("the Militia Captain", "Thoren Oakshield")
        {
        }

        public ThorenOakshield(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 95, 80);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Tanned, weathered skin
            HairItemID = 0x2048; // Long hair
            HairHue = 1109; // Ash-grey
            FacialHairItemID = 0x203B; // Full beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 2101, Name = "Scorched Militia Plate" }); // Charred steel hue
            AddItem(new PlateLegs() { Hue = 2101, Name = "Charred Greaves" });
            AddItem(new LeatherGloves() { Hue = 1837, Name = "Oakshield’s Bracers" });
            AddItem(new StuddedGorget() { Hue = 2208, Name = "Battleworn Gorget" });
            AddItem(new HalfApron() { Hue = 2117, Name = "Ember-Stained Apron" });
            AddItem(new Boots() { Hue = 1815, Name = "Ashen Boots" });

            AddItem(new Broadsword() { Hue = 1920, Name = "Inferno-Biter" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Militia Pack";
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
