using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ChainsOfTheSacredQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Chains of the Sacred"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Morgath Dawnshield*, the Druidic Envoy tasked with guarding the scorched leylines of Death Glutch.\n\n" +
                    "He stands in silent vigil beside a withered totem, his robes scorched at the hem, hands wrapped around a talisman that hums faintly with sorrow.\n\n" +
                    "“The Kirin was once a guardian—majestic, pure. But **I** helped bind it, long ago, under misguided orders to contain its power.”\n\n" +
                    "“Now it suffers, twisted in the shadows beneath Malidor’s northern tower. Its agony tears through the ley, warping the land we strive to heal.”\n\n" +
                    "“The talisman I hold? It once chained it. I thought I was preserving balance. Instead, I’ve wrought torment.”\n\n" +
                    "**Slay the Bound Kirin. Free it from this torment. Let the ley-lines breathe again.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then leave me to this ruin... but know that each breath the Kirin draws tears this land deeper into shadow.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it suffers? I feel it... every tremor, every cry. The chains tighten by the hour.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You have freed it... and in doing so, perhaps freed me as well.\n\n" +
                       "Take this: *RareSausage.* A humble gift, perhaps, but prepared with herbs from ley-kissed soil, now healing.\n\n" +
                       "May its taste remind you that small things, too, hold power in this world.";
            }
        }

        public ChainsOfTheSacredQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BoundKirin), "the Bound Kirin", 1));
            AddReward(new BaseReward(typeof(RareSausage), 1, "RareSausage"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Chains of the Sacred'!");
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

    public class MorgathDawnshield : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ChainsOfTheSacredQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist()); // Closest fit for a druidic envoy with ley-line care
        }

        [Constructable]
        public MorgathDawnshield()
            : base("the Druidic Envoy", "Morgath Dawnshield")
        {
        }

        public MorgathDawnshield(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 75, 90);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // Weathered desert tone
            HairItemID = 0x2049; // Long Hair
            HairHue = 1150; // Ash-grey
            FacialHairItemID = 0x203C; // Long Beard
            FacialHairHue = 1150; // Matching ash-grey
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 2425, Name = "Ley-Touched Robe" }); // Faded green, marked with runes
            AddItem(new LeatherGorget() { Hue = 2402, Name = "Ashen Gorget" }); // Burnt leather, ley-protective
            AddItem(new Sandals() { Hue = 2101, Name = "Wanderer's Sandals" }); // Earthen toned
            AddItem(new BodySash() { Hue = 2430, Name = "Druid’s Binding Sash" }); // Embroidered with leyline symbols
            AddItem(new HoodedShroudOfShadows() { Hue = 0, Name = "Twilight Hood" }); // Black, to conceal shame and focus in meditation

            Backpack backpack = new Backpack();
            backpack.Hue = 1154;
            backpack.Name = "Leykeeper's Pack";
            AddItem(backpack);

            // Carries the talisman of regret
            AddItem(new MagicWand() { Hue = 1165, Name = "Talisman of the Bound" });
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
