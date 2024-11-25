using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a big cat tamer")]
    public class BigCatTamer : BaseCreature
    {
        private TimeSpan m_CommandDelay = TimeSpan.FromSeconds(15.0); // time between commands
        public DateTime m_NextCommandTime;

        [Constructable]
        public BigCatTamer() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Big Cat Tamer";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Big Cat Tamer";
            }

            Item hat = new WideBrimHat();
            Item robe = new Robe(Utility.RandomNeutralHue());
            Item shoes = new Shoes(Utility.RandomNeutralHue());
            AddItem(hat);
            AddItem(robe);
            AddItem(shoes);

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

            SetStr(700, 1000);
            SetDex(150, 200);
            SetInt(150, 200);

            SetHits(500, 700);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Fire, 25);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 50);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.Magery, 85.1, 100.0);
            SetSkill(SkillName.MagicResist, 80.0, 95.0);
            SetSkill(SkillName.Tactics, 75.0, 90.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            m_NextCommandTime = DateTime.Now + m_CommandDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextCommandTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
                {
                    Say(true, "Attack, my fierce companions!");

                    foreach (Mobile m in this.GetMobilesInRange(10))
                    {
                        if (m is BigCat)
                        {
                            BigCat cat = (BigCat)m;
                            cat.Combatant = combatant;
                            cat.Pounce(combatant);
                        }
                    }

                    m_NextCommandTime = DateTime.Now + m_CommandDelay;
                }
            }

            base.OnThink();
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.FilthyRich);
            PackItem(new Bandage(Utility.RandomMinMax(10, 20)));
        }

        public BigCatTamer(Serial serial) : base(serial)
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

    public class BigCat : BaseCreature
    {
        [Constructable]
        public BigCat() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0xD6;
            Name = "a big cat";

            SetStr(300, 400);
            SetDex(150, 200);
            SetInt(50, 75);

            SetHits(200, 300);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 50.0, 75.0);
            SetSkill(SkillName.Tactics, 75.0, 90.0);
            SetSkill(SkillName.Wrestling, 75.0, 90.0);

            Fame = 2500;
            Karma = -2500;

            VirtualArmor = 20;
        }

        public void Pounce(Mobile target)
        {
            if (target != null && target.Alive)
            {
                Say("*The big cat pounces!*");
                target.Damage(Utility.RandomMinMax(30, 50), this);
                if (Utility.RandomDouble() < 0.25)
                {
                    target.Paralyze(TimeSpan.FromSeconds(2.0));
                }
            }
        }

        public BigCat(Serial serial) : base(serial)
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
