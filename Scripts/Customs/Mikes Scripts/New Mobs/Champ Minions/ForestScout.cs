using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a forest scout")]
    public class ForestScout : BaseCreature
    {
        private TimeSpan m_AmbushDelay = TimeSpan.FromSeconds(15.0); // time between ambushes
        public DateTime m_NextAmbushTime;

        [Constructable]
        public ForestScout() : base(AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Forest Scout";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Forest Scout";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item shirt = new Shirt(Utility.RandomNeutralHue());
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new Boots(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(shirt);
            AddItem(pants);
            AddItem(boots);

            Item weapon = new Hatchet();
            AddItem(weapon);
            weapon.Movable = false;

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(500, 800);
            SetDex(300, 500);
            SetInt(100, 200);

            SetHits(400, 600);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Anatomy, 60.0, 80.0);
            SetSkill(SkillName.Archery, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Hiding, 80.0, 100.0);
            SetSkill(SkillName.Stealth, 80.0, 100.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 45;

            m_NextAmbushTime = DateTime.Now + m_AmbushDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextAmbushTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
                {
                    this.Say(true, "You can't hide from the forest!");

                    combatant.FixedParticles(0x376A, 10, 15, 5032, EffectLayer.Head);
                    combatant.PlaySound(0x1E5);

                    combatant.Damage(Utility.RandomMinMax(20, 30), this);

                    m_NextAmbushTime = DateTime.Now + m_AmbushDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(100, 150);
            AddLoot(LootPack.Average);

            this.Say(true, "The forest... it will reclaim me...");
        }

        public override int Damage(int amount, Mobile from)
        {
            if (Utility.RandomBool())
            {
                this.Say(true, "You can't defeat nature!");

                from.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                from.PlaySound(0x1F2);
            }

            return base.Damage(amount, from);
        }

        public ForestScout(Serial serial) : base(serial)
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
