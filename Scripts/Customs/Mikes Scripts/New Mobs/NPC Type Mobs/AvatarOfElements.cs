using System;
using Server.Items;
using Server.Spells;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of The Avatar of Elements")]
    public class AvatarOfElements : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between Avatar's speech
        private DateTime m_NextSpeechTime;

        [Constructable]
        public AvatarOfElements() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = 0x4001; // You can modify this for a unique hue if you want.
            Body = 0x190;
            Name = "The Avatar of Elements";

            // Equipment
            Item robe = new Robe();
            robe.Hue = 0x485; // Setting the color to something elemental.
            AddItem(robe);

            // Abilities and stats
            SetStr(800, 1200);
            SetDex(177, 255);
            SetInt(151, 250);

            SetHits(600, 1000);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            // Random skills
            SetSkill(SkillName.Anatomy, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Archery, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.ArmsLore, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Bushido, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Chivalry, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Fencing, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Lumberjacking, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Ninjitsu, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Parry, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Swords, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Tactics, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Wrestling, Utility.RandomMinMax(50, 100));

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 58;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime && Combatant != null)
            {
                Mobile combatant = Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: Say("Feel the gusts of wind!"); SummonElementalCreature("air"); break;
                        case 1: Say("Face the fury of the flames!"); SummonElementalCreature("fire"); break;
                        case 2: Say("The earth shall crush you!"); SummonElementalCreature("earth"); break;
                        case 3: Say("Be overwhelmed by the waters!"); SummonElementalCreature("water"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        public void SummonElementalCreature(string type)
        {
            BaseCreature creature;

            switch (type)
            {
                case "air": creature = new AirElemental(); break;
                case "fire": creature = new FireElemental(); break;
                case "earth": creature = new EarthElemental(); break;
                case "water": creature = new WaterElemental(); break;
                default: return;
            }

            creature.MoveToWorld(this.Location, this.Map);
            creature.Combatant = this.Combatant;
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: Say("Feel the gusts of wind!"); SummonElementalCreature("air"); break;
                        case 1: Say("Face the fury of the flames!"); SummonElementalCreature("fire"); break;
                        case 2: Say("The earth shall crush you!"); SummonElementalCreature("earth"); break;
                        case 3: Say("Be overwhelmed by the waters!"); SummonElementalCreature("water"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public override void GenerateLoot()
        {
            PackGem(2); // Gives 2 random gems.
            PackGold(300, 500);
            AddLoot(LootPack.UltraRich, 2);
        }

        public AvatarOfElements(Serial serial) : base(serial)
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
