using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a rune caster")]
    public class RuneCaster : BaseCreature
    {
        private TimeSpan m_RuneDelay = TimeSpan.FromSeconds(10.0); // time between rune placements
        public DateTime m_NextRuneTime;

        [Constructable]
        public RuneCaster() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Rune Caster";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Rune Caster";
            }

            Item robe = new Robe(Utility.RandomNeutralHue());
            AddItem(robe);
            
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);

            SetStr(600, 800);
            SetDex(150, 200);
            SetInt(200, 300);

            SetHits(400, 600);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 20, 40);
            SetResistance(ResistanceType.Energy, 20, 40);

            SetSkill(SkillName.EvalInt, 80.0, 100.0);
            SetSkill(SkillName.Magery, 90.0, 100.0);
            SetSkill(SkillName.Meditation, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 85.0, 100.0);
            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 40;

            m_NextRuneTime = DateTime.Now + m_RuneDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextRuneTime)
            {
                PlaceRune();
                m_NextRuneTime = DateTime.Now + m_RuneDelay;
            }

            base.OnThink();
        }

        public void PlaceRune()
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                Rune rune = new Rune();
                rune.MoveToWorld(this.Location, this.Map);
                this.Say(true, "A rune has been placed!");

                Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerStateCallback(DetonateRune), rune);
            }
        }

        public void DetonateRune(object state)
        {
            Rune rune = state as Rune;
            if (rune != null && !rune.Deleted)
            {
                rune.Explode();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(150, 200);
            AddLoot(LootPack.Rich);

            this.Say(true, "My runes... failed me...");
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));
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
                        case 0: this.Say(true, "You will pay for this!"); break;
                        case 1: this.Say(true, "My runes will avenge me!"); break;
                        case 2: this.Say(true, "Feel the power of my runes!"); break;
                        case 3: this.Say(true, "This is not the end!"); break;
                    }
                    
                    m_NextRuneTime = DateTime.Now + m_RuneDelay;
                }
            }
                
            return base.Damage(amount, from);
        }

        public RuneCaster(Serial serial) : base(serial)
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

    public class Rune : Item
    {
        [Constructable]
        public Rune() : base(0x1F14) // Assuming a rune graphic
        {
            Movable = false;
            Hue = 1153;
        }

        public void Explode()
        {
            this.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "The rune explodes!");
            // Damage or debuff logic here
            this.Delete();
        }

        public Rune(Serial serial) : base(serial)
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
