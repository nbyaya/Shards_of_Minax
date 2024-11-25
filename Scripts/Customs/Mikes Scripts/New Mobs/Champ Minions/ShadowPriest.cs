using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a shadow priest")]
    public class ShadowPriest : BaseCreature
    {
        private TimeSpan m_CurseDelay = TimeSpan.FromSeconds(10.0); // time between curses
        public DateTime m_NextCurseTime;

        [Constructable]
        public ShadowPriest() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = 0x455;
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = "the Shadow Priest";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = "the Shadow Priest";
            }

            Item robe = new Robe(0x455);
            Item sandals = new Sandals(0x455);
            Item hood = new HoodedShroudOfShadows();
            hood.Hue = 0x455;

            AddItem(robe);
            AddItem(sandals);
            AddItem(hood);

            SetStr(150, 200);
            SetDex(100, 150);
            SetInt(250, 300);

            SetHits(300, 400);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 100.1, 110.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 50.1, 75.0);
            SetSkill(SkillName.Wrestling, 60.1, 80.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            m_NextCurseTime = DateTime.Now + m_CurseDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextCurseTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    CastCurse(combatant);
                    m_NextCurseTime = DateTime.Now + m_CurseDelay;
                }
            }
        }

        public void CastCurse(Mobile target)
        {
            if (target != null && target.Alive && target.Map == this.Map && target.InRange(this, 12))
            {
                this.Say(true, "Feel the shadow's grasp!");
                // Implement curse effect here, for example:
                target.Paralyze(TimeSpan.FromSeconds(5.0));
                target.SendMessage("You feel a dark force crippling you!");
                // Additional curse effects can be added here
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.Rich);
            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 20)));
        }

        public override int Damage(int amount, Mobile from)
        {
            int damage = base.Damage(amount, from);

            if (from != null && from.Map == this.Map && from.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    this.Say(true, "You cannot escape the shadows!");
                }
            }

            return damage;
        }

        public ShadowPriest(Serial serial) : base(serial)
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
