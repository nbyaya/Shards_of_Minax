using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a relativist")]
    public class Relativist : BaseCreature
    {
        private TimeSpan m_TimeShiftDelay = TimeSpan.FromSeconds(10.0); // time between time shifts
        public DateTime m_NextTimeShift;

        [Constructable]
        public Relativist() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Relativist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Relativist";
            }

            Item robe = new Robe();
            robe.Hue = Utility.RandomDyedHue();
            AddItem(robe);

            Item sandals = new Sandals();
            sandals.Hue = Utility.RandomDyedHue();
            AddItem(sandals);

            Item staff = new BlackStaff();
            AddItem(staff);
            staff.Movable = false;

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(500, 700);
            SetDex(150, 200);
            SetInt(400, 500);

            SetHits(400, 600);

            SetDamage(8, 14);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 70.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 50.0, 70.0);
            SetSkill(SkillName.Wrestling, 50.0, 70.0);

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 50;

            m_NextTimeShift = DateTime.Now + m_TimeShiftDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextTimeShift)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    // Slow enemies in range
                    if (Utility.RandomBool())
                    {
                        combatant.SendMessage("You feel your movements slowing down...");
                        combatant.Paralyze(TimeSpan.FromSeconds(5.0));
                    }

                    m_NextTimeShift = DateTime.Now + m_TimeShiftDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.FilthyRich);

            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 20)));
        }
        
        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    combatant.SendMessage("The Relativist manipulates the flow of time, making you feel sluggish!");
                    combatant.Paralyze(TimeSpan.FromSeconds(3.0));
                }
            }
                
            return base.Damage(amount, from);
        }

        public Relativist(Serial serial) : base(serial)
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
