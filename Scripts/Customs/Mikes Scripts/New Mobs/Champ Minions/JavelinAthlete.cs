using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a javelin athlete")]
    public class JavelinAthlete : BaseCreature
    {
        private TimeSpan m_ThrowDelay = TimeSpan.FromSeconds(5.0); // time between javelin throws
        public DateTime m_NextThrowTime;

        [Constructable]
        public JavelinAthlete() : base(AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Javelin Athlete";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Javelin Athlete";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item tunic = new Tunic();
            Item sandals = new Sandals();
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(tunic);
            AddItem(sandals);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(500, 700);
            SetDex(200, 250);
            SetInt(100, 150);

            SetHits(400, 600);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Anatomy, 60.0, 80.0);
            SetSkill(SkillName.Archery, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 70.0, 90.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 30;

            m_NextThrowTime = DateTime.Now + m_ThrowDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextThrowTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
                {
                    this.Say(true, "Feel the sting of my javelin!");
                    ThrowJavelin(combatant);

                    m_NextThrowTime = DateTime.Now + m_ThrowDelay;
                }

                base.OnThink();
            }
        }

        private void ThrowJavelin(Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive || target.Map != this.Map)
                return;

            if (this.CanBeHarmful(target))
            {
                this.DoHarmful(target);
                target.Damage(Utility.RandomMinMax(20, 35), this);
            }
        }

        public override void GenerateLoot()
        {
            PackGold(150, 200);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My javelins... failed me..."); break;
                case 1: this.Say(true, "You got lucky..."); break;
            }


        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "That won't stop me!"); break;
                        case 1: this.Say(true, "Is that your best shot?"); break;
                        case 2: this.Say(true, "I'll return the favor!"); break;
                        case 3: this.Say(true, "You'll regret that!"); break;
                    }

                    m_NextThrowTime = DateTime.Now + m_ThrowDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public JavelinAthlete(Serial serial) : base(serial)
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
