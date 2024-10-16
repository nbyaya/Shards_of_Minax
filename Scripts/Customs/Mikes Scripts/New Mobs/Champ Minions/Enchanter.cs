using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of an enchanter")]
    public class Enchanter : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between enchanter speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Enchanter() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Enchanter";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Enchanter";
            }

            if (Utility.RandomBool())
            {
                Item robe = new Robe();
                AddItem(robe);
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            SetStr(400, 600);
            SetDex(100, 150);
            SetInt(500, 700);

            SetHits(300, 500);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 50.1, 60.0);
            SetSkill(SkillName.MagicResist, 70.5, 80.0);
            SetSkill(SkillName.Tactics, 60.1, 70.0);
            SetSkill(SkillName.Wrestling, 50.1, 60.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 40;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Feel the power of my magic!"); break;
                        case 1: this.Say(true, "I will weaken you!"); break;
                        case 2: this.Say(true, "My allies will be strengthened!"); break;
                        case 3: this.Say(true, "You cannot withstand my enchantments!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(150, 200);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My magic fades..."); break;
                case 1: this.Say(true, "The enchantments... fail..."); break;
            }

            PackItem(new Nightshade(Utility.RandomMinMax(5, 15)));
        }

        public void CastBuffOrDebuff()
        {
            if (Combatant != null && Combatant.Alive)
            {
                if (Utility.RandomBool())
                {
                    // Cast a buff on a nearby ally
                    Mobile ally = FindAlly();
                    if (ally != null)
                    {
                        this.Say(true, "Be empowered, my ally!");
                        // Add code to apply a buff to the ally
                    }
                }
                else
                {
                    // Cast a debuff on the enemy
                    this.Say(true, "Be weakened, enemy!");
                    // Add code to apply a debuff to the enemy
                }
            }
        }

        private Mobile FindAlly()
        {
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m is BaseCreature && ((BaseCreature)m).Controlled && ((BaseCreature)m).ControlMaster == this.ControlMaster)
                {
                    return m;
                }
            }
            return null;
        }

        public Enchanter(Serial serial) : base(serial)
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
