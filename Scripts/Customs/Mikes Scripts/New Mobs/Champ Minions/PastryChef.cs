using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a pastry chef")]
    public class PastryChef : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between chef speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public PastryChef() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Pastry Chef";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Pastry Chef";
            }

            Item hat = new Bonnet();
            Item apron = new FullApron();
            Item shoes = new Sandals(Utility.RandomNeutralHue());

            hat.Hue = Utility.RandomBrightHue();
            apron.Hue = Utility.RandomNeutralHue();
            hat.Movable = false;
            apron.Movable = false;
            shoes.Movable = false;

            AddItem(hat);
            AddItem(apron);
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

            SetStr(600, 800);
            SetDex(120, 150);
            SetInt(150, 200);

            SetHits(400, 600);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 50);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 50.1, 75.0);
            SetSkill(SkillName.EvalInt, 80.1, 100.0);
            SetSkill(SkillName.Magery, 80.1, 100.0);
            SetSkill(SkillName.Meditation, 50.1, 75.0);
            SetSkill(SkillName.MagicResist, 85.0, 100.0);
            SetSkill(SkillName.Tactics, 75.1, 100.0);
            SetSkill(SkillName.Wrestling, 75.1, 100.0);

            Fame = 5000;
            Karma = -5000;

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
                        case 0: this.Say(true, "Taste my pastries!"); break;
                        case 1: this.Say(true, "You'll get stuck in my dough!"); break;
                        case 2: this.Say(true, "Feel the sweetness of defeat!"); break;
                        case 3: this.Say(true, "I'll bake you into submission!"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGold(150, 200);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My pastries... failed me..."); break;
                case 1: this.Say(true, "You... ruined my recipe..."); break;
            }

            PackItem(new BreadLoaf(Utility.RandomMinMax(2, 5)));
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Is that all you got?"); break;
                        case 1: this.Say(true, "You'll need more than that!"); break;
                        case 2: this.Say(true, "My pastries are tougher!"); break;
                        case 3: this.Say(true, "You won't best me!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            base.OnDamage(amount, from, willKill);
        }

        public void ThrowStickyPastry(Mobile target)
        {
            if (target != null && target.Alive && !target.IsDeadBondedPet)
            {
                target.SendMessage("You've been hit by a sticky pastry!");
                target.Freeze(TimeSpan.FromSeconds(5.0)); // Slow the target for 5 seconds
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.25) // 25% chance to throw a sticky pastry
            {
                ThrowStickyPastry(attacker);
            }
        }

        public PastryChef(Serial serial) : base(serial)
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
