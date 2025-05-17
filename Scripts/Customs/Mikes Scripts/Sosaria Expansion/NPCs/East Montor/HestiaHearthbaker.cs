using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class NightBatsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Night Bats"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Hestia Hearthbaker*, hands dusted in flour, eyes blazing with frustration amidst the scent of freshly baked bread.\n\n" +
                    "She gestures wildly to a vent above, where soot and acrid residue taint the bakery's warmth.\n\n" +
                    "“Every night, those wretched **Draconic Mongbats** swarm my ovens! Their droppings ruin the dough, foul the air, and choke the very fire I need to bake. \n\n" +
                    "And their wings—sharp as a butcher's cleaver—tear through the vents, scraping like nails on slate!”\n\n" +
                    "“They roost deep within the **Caves of Drakkon**, drawn by the heat and the scent of yeast. I've heard tales of their kind, twisted by dragon blood, maddened by the caves' molten heart.”\n\n" +
                    "**Drive them out. Slay the Draconic Mongbat** that leads them. Let my ovens breathe free again.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Fine then. But don't expect fresh bread when the air still reeks of bat filth.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You haven't dealt with them yet? My loaves suffer, and the nights grow louder.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You've done it? The vents are clear, the air sweet again! \n\n" +
                       "**Take this:** it's cluttered, yes, but every note, recipe, and secret I hold is written here. Maybe you'll find something useful amongst the crumbs.";
            }
        }

        public NightBatsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DraconicMongbat), "Draconic Mongbat", 1));
            AddReward(new BaseReward(typeof(ClutteredDesk), 1, "Cluttered Desk"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Night Bats'!");
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

    public class HestiaHearthbaker : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(NightBatsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBaker());
        }

        [Constructable]
        public HestiaHearthbaker()
            : base("the Breadmaker", "Hestia Hearthbaker")
        {
        }

        public HestiaHearthbaker(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 35);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C; // Wavy Hair
            HairHue = 1150; // Ash Blonde
        }

        public override void InitOutfit()
        {
            AddItem(new PlainDress() { Hue = 2101, Name = "Baker's Dress" }); // Warm Honey
            AddItem(new HalfApron() { Hue = 2302, Name = "Flour-Streaked Apron" }); // Off-white
            AddItem(new Sandals() { Hue = 2106, Name = "Hearth-Worn Sandals" }); // Charcoal Gray
            AddItem(new ChefsToque() { Hue = 2109, Name = "Oven-Kissed Toque" }); // Soft Cream
            AddItem(new BodySash() { Hue = 2124, Name = "Spice Sash" }); // Deep Cinnamon

            Backpack backpack = new Backpack();
            backpack.Hue = 1153; // Bakery Brown
            backpack.Name = "Baker's Satchel";
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
