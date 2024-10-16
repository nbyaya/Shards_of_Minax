using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a crime scene tech")]
    public class CrimeSceneTech : BaseCreature
    {
        private TimeSpan m_GadgetDelay = TimeSpan.FromSeconds(15.0); // time between gadget uses
        public DateTime m_NextGadgetTime;

        [Constructable]
        public CrimeSceneTech() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Crime Scene Tech";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Crime Scene Tech";
            }

            Item coat = new Robe();
            AddItem(coat);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new Shoes(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(pants);
            AddItem(boots);

            SetStr(600, 800);
            SetDex(150, 200);
            SetInt(400, 500);

            SetHits(400, 600);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 50);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 80.1, 100.0);
            SetSkill(SkillName.Magery, 85.1, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 90.5, 120.0);
            SetSkill(SkillName.Tactics, 70.1, 90.0);
            SetSkill(SkillName.Wrestling, 70.1, 90.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            m_NextGadgetTime = DateTime.Now + m_GadgetDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextGadgetTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int gadget = Utility.Random(2);

                    switch (gadget)
                    {
                        case 0:
                            this.Say(true, "Deploying immobilizing gadget!");
                            combatant.Paralyze(TimeSpan.FromSeconds(5));
                            break;
                        case 1:
                            this.Say(true, "Deploying disorienting gadget!");
                            combatant.FixedEffect(0x376A, 10, 16);
                            combatant.PlaySound(0x1E2);
                            break;
                    }

                    m_NextGadgetTime = DateTime.Now + m_GadgetDelay;
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
                case 0: this.Say(true, "My gadgets... failed me..."); break;
                case 1: this.Say(true, "This isn't over..."); break;
            }

            PackItem(new BlackPearl(Utility.RandomMinMax(10, 20)));
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
                        case 0: this.Say(true, "You can't break through my gadgets!"); break;
                        case 1: this.Say(true, "Is that all your strength?"); break;
                        case 2: this.Say(true, "I've handled worse threats!"); break;
                        case 3: this.Say(true, "You're no match for my tech!"); break;
                    }

                    m_NextGadgetTime = DateTime.Now + m_GadgetDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public CrimeSceneTech(Serial serial) : base(serial)
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
