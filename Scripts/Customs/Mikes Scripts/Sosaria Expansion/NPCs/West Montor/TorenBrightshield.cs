using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class PurgeTheHellfire : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Defeat the HellfireProtector"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Toren Brightshield*, Temple Guardian of West Montor’s Sacred Flame Shrine.\n\n" +
                    "His armor glows faintly, enchanted against the infernal heat, yet his eyes reflect the burden of failing light.\n\n" +
                    "“Our dawn lanterns... choked by infernal embers. We rekindle them each morning, but now... now they smolder in darkness.”\n\n" +
                    "“The *HellfireProtector*, spawned from the Gate of Hell, has anchored itself near our shrine. Its presence corrupts the sacred flame, threatening to extinguish the light our people rely upon.”\n\n" +
                    "“I trained in East Montor, among the fire priests, to guard against such threats—but this creature’s power defies my rites.”\n\n" +
                    "**Purge the HellfireProtector**. Restore our light. Without it, the dawn will falter—and shadows will reign.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the flames survive this trial without you... but I fear they shall not.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The protector still thrives? I feel its heat from here... it gnaws at the edges of our shrine’s light.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The sacred flames... they burn true once more. You’ve lifted the veil of ash that choked our dawn.\n\n" +
                       "Take this: *LargeTome*. A record of rites, teachings, and strength. May it guide you, as you have guided our light.";
            }
        }

        public PurgeTheHellfire() : base()
        {
            AddObjective(new SlayObjective(typeof(HellfireProtector), "HellfireProtector", 1));
            AddReward(new BaseReward(typeof(LargeTome), 1, "LargeTome"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Defeat the HellfireProtector'!");
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

    public class TorenBrightshield : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(PurgeTheHellfire) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHolyMage()); // Reflects his Temple Guardian role
        }

        [Constructable]
        public TorenBrightshield()
            : base("the Temple Guardian", "Toren Brightshield")
        {
        }

        public TorenBrightshield(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 70);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Warm bronze skin
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Flame-red hair
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 1358, Name = "Emberforged Cuirass" }); // Glowing orange
            AddItem(new PlateLegs() { Hue = 1255, Name = "Ashen Greaves" }); // Smoky steel
            AddItem(new PlateGloves() { Hue = 1157, Name = "Guardian's Gauntlets" }); // Dark iron
            AddItem(new PlateGorget() { Hue = 1255, Name = "Flame-Ward Gorget" });
            AddItem(new WingedHelm() { Hue = 1358, Name = "Helm of the Dawn Sentinel" }); // Radiant orange

            AddItem(new Cloak() { Hue = 1153, Name = "Cloak of the Sacred Flame" }); // Deep red cloak
            AddItem(new BodySash() { Hue = 1358, Name = "Sash of Purity" }); // Glowing orange sash

            AddItem(new Broadsword() { Hue = 1257, Name = "Brightshield Blade" }); // Fire-hued blade
            AddItem(new MetalShield() { Hue = 1157, Name = "Holy Ward" }); // Dark shield engraved with holy runes

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Brightshield’s Pack";
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
