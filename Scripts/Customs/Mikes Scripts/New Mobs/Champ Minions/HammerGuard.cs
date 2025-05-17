using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a hammer guard")]
    public class HammerGuard : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between guard speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public HammerGuard() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Hammer Guard";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Hammer Guard";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item armor = new PlateChest();
            Item pants = new PlateLegs();

            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            Item weapon = new WarHammer();
            weapon.Movable = false;

            AddItem(hair);
            AddItem(armor);
            AddItem(pants);

            AddItem(weapon);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(1000, 1200);
            SetDex(150, 200);
            SetInt(50, 75);

            SetHits(800, 1000);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 75.1, 100.0);
            SetSkill(SkillName.MagicResist, 80.1, 100.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Swords, 90.1, 100.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 60;

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
                        case 0: this.Say(true, "Feel the power of my hammer!"); break;
                        case 1: this.Say(true, "You shall not pass!"); break;
                        case 2: this.Say(true, "Back off, intruder!"); break;
                        case 3: this.Say(true, "Prepare to be crushed!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (from != null && from != this && from.Map == this.Map && from.InRange(this, 2))
            {
                if (Utility.RandomDouble() < 0.25)
                {
                    this.Say(true, "Feel my wrath!");
                    from.MoveToWorld(new Point3D(from.X + Utility.RandomMinMax(-1, 1), from.Y + Utility.RandomMinMax(-1, 1), from.Z), from.Map);
                    from.PlaySound(0x1F7);
                }
            }
        }

        public override void GenerateLoot()
        {
            PackGold(300, 400);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My watch... ends here..."); break;
                case 1: this.Say(true, "You will... be avenged..."); break;
            }

            PackItem(new IronIngot(Utility.RandomMinMax(10, 20)));
        }

        public HammerGuard(Serial serial) : base(serial)
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
