using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a shield maiden")]
    public class ShieldMaiden : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between shield bearer speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public ShieldMaiden() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = 0x190; // Assuming male for this example, change to 0x191 for female if needed
            Name = "Shield Bearer";
			Team = 2;

            Item shield = new HeaterShield();
            shield.Movable = false;
            AddItem(shield);

            Item armor = new PlateChest();
            armor.Movable = false;
            AddItem(armor);

            Item helm = new PlateHelm();
            helm.Movable = false;
            AddItem(helm);

            Item legs = new PlateLegs();
            legs.Movable = false;
            AddItem(legs);

            Item gloves = new PlateGloves();
            gloves.Movable = false;
            AddItem(gloves);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            SetStr(1000, 1200);
            SetDex(60, 80);
            SetInt(40, 60);

            SetHits(1000, 1200);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 75.1, 100.0);
            SetSkill(SkillName.MagicResist, 90.5, 120.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);
            SetSkill(SkillName.Parry, 100.1, 120.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 70;

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
                        case 0: this.Say(true, "You shall not pass!"); break;
                        case 1: this.Say(true, "My shield will protect me!"); break;
                        case 2: this.Say(true, "I am invincible!"); break;
                        case 3: this.Say(true, "Feel the power of my shield!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.FilthyRich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My shield... has fallen..."); break;
                case 1: this.Say(true, "You have bested me..."); break;
            }

            PackItem(new IronIngot(Utility.RandomMinMax(10, 20)));
        }

        public override int Damage(int amount, Mobile from)
        {
            if (Utility.RandomDouble() < 0.5) // 50% chance to block the attack
            {
                this.Say(true, "Blocked!");
                return 0;
            }

            return base.Damage(amount, from);
        }

        public ShieldMaiden(Serial serial) : base(serial)
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
