using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a choir singer")]
    public class ChoirSinger : BaseCreature
    {
        private TimeSpan m_AbilityDelay = TimeSpan.FromSeconds(15.0); // time between abilities
        public DateTime m_NextAbilityTime;

        [Constructable]
        public ChoirSinger() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Choir Singer";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Choir Singer";
            }

            Item robe = new Robe(Utility.RandomNeutralHue());
            Item shoes = new Sandals(Utility.RandomNeutralHue());
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(robe);
            AddItem(shoes);
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(300, 500);
            SetDex(150, 200);
            SetInt(200, 300);

            SetHits(400, 600);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Meditation, 75.0, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 100.0);
            SetSkill(SkillName.Magery, 75.0, 100.0);
            SetSkill(SkillName.EvalInt, 75.0, 100.0);
            SetSkill(SkillName.Wrestling, 50.0, 75.0);

            Fame = 3000;
            Karma = 3000;

            VirtualArmor = 30;

            m_NextAbilityTime = DateTime.Now + m_AbilityDelay;
        }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextAbilityTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int ability = Utility.Random(2);

                    switch (ability)
                    {
                        case 0:
                            this.Say(true, "Feel the soothing power of my voice!");
                            // Logic to soothe allies
                            SootheAllies();
                            break;
                        case 1:
                            this.Say(true, "Be disoriented by my song!");
                            // Logic to disorient enemies
                            DisorientEnemies(combatant);
                            break;
                    }

                    m_NextAbilityTime = DateTime.Now + m_AbilityDelay;
                }

                base.OnThink();
            }
        }

        private void SootheAllies()
        {
            IPooledEnumerable eable = this.GetMobilesInRange(8);
            foreach (Mobile m in eable)
            {
                if (m is BaseCreature && ((BaseCreature)m).Controlled && ((BaseCreature)m).ControlMaster == this.ControlMaster)
                {
                    m.Heal(Utility.RandomMinMax(10, 30));
                    m.SendMessage("You feel soothed by the choir singer's voice.");
                }
            }
            eable.Free();
        }

        private void DisorientEnemies(Mobile target)
        {
            target.Paralyze(TimeSpan.FromSeconds(Utility.RandomMinMax(1, 3)));
            target.SendMessage("You feel disoriented by the choir singer's song.");
        }

        public ChoirSinger(Serial serial) : base(serial)
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
