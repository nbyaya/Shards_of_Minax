using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a herbalist poisoner")]
    public class HerbalistPoisoner : BaseCreature
    {
        private TimeSpan m_PoisonDelay = TimeSpan.FromSeconds(30.0); // time between poison zones
        public DateTime m_NextPoisonTime;

        [Constructable]
        public HerbalistPoisoner() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = Utility.RandomList(0x191, 0x190);
            Name = NameList.RandomName(Body == 0x191 ? "female" : "male");
            Title = " the Herbalist Poisoner";
			Team = 2;

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            Item robe = new Robe(Utility.RandomGreenHue());
            Item sandals = new Sandals(Utility.RandomNeutralHue());
            AddItem(hair);
            AddItem(robe);
            AddItem(sandals);

            SetStr(300, 400);
            SetDex(100, 150);
            SetInt(250, 300);

            SetHits(200, 300);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 50.1, 75.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 50.1, 75.0);
            SetSkill(SkillName.MagicResist, 85.5, 100.0);
            SetSkill(SkillName.Tactics, 75.1, 90.0);
            SetSkill(SkillName.Wrestling, 60.1, 80.0);
            SetSkill(SkillName.Poisoning, 90.1, 100.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            m_NextPoisonTime = DateTime.Now + m_PoisonDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextPoisonTime)
            {
                if (Combatant != null && Combatant.Map == this.Map)
                {
                    Say(true, "Feel the sting of nature's wrath!");

                    // Create a poison gas cloud at the combatant's location
                    PoisonGasTrap poisonGas = new PoisonGasTrap();
                    poisonGas.MoveToWorld(Combatant.Location, Combatant.Map);

                    m_NextPoisonTime = DateTime.Now + m_PoisonDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(100, 150);
            AddLoot(LootPack.Rich);

            PackItem(new GreaterPoisonPotion());
        }

        public override int Damage(int amount, Mobile from)
        {
            if (Combatant != null && Combatant.Map == this.Map)
            {
                if (Utility.RandomBool())
                {
                    Say(true, "Nature's toxins will be your end!");
                }
            }

            return base.Damage(amount, from);
        }

        public HerbalistPoisoner(Serial serial) : base(serial)
        {
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

    public class PoisonGasTrap : Item
    {
        [Constructable]
        public PoisonGasTrap() : base(0x3709)
        {
            Movable = false;
            Hue = 0x3F;
            Name = "a poison gas cloud";

            Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerCallback(Delete));
        }

        public PoisonGasTrap(Serial serial) : base(serial)
        {
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
