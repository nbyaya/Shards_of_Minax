using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Hulk Hogan")]
    public class UltimateMasterWrestling : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterWrestling()
            : base(AIType.AI_Melee)
        {
            Name = "Hulk Hogan";
            Title = "The Wrestling Legend";
            Body = 0x190;
            Hue = 0x83EA;

            SetStr(500, 700);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(15000);
            SetMana(1000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Wrestling, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 75;

            AddItem(new FancyShirt(0x1));
            AddItem(new LongPants(0x1));
            AddItem(new Boots());
            AddItem(new Bandana(0x21E));

            HairItemID = 0x203B; // Long Hair
            HairHue = 0x93;

            m_NextAbilityTime = DateTime.Now;
        }

        public UltimateMasterWrestling(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(ChampionshipBelt), typeof(WrestlersGloves) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(WrestlingTome), typeof(HealingPotion) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(ChampionshipBelt), typeof(WrestlersStatue) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 6);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.Wrestling, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ChampionshipBelt());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new WrestlersGloves());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: BodySlam(defender); break;
                    case 1: Grapple(defender); break;
                    case 2: HulkSmash(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void BodySlam(Mobile defender)
        {
            if (defender != null)
            {
                defender.Damage(Utility.RandomMinMax(80, 100), this);
                defender.Paralyze(TimeSpan.FromSeconds(4.0));

                defender.FixedParticles(0x37B9, 1, 14, 0x26B8, 0x3F, 0x7, EffectLayer.Head);
                defender.PlaySound(0x213);
            }
        }

        public void Grapple(Mobile defender)
        {
            if (defender != null)
            {
                defender.Paralyze(TimeSpan.FromSeconds(8.0));

                defender.FixedParticles(0x37B9, 1, 14, 0x26B8, 0x3F, 0x7, EffectLayer.Waist);
                defender.PlaySound(0x21A);
            }
        }

        public void HulkSmash()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoHarmful(m);

                int damage = Utility.RandomMinMax(100, 150);

                AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

                m.FixedParticles(0x36B0, 1, 14, 0x26B8, 0x3F, 0x7, EffectLayer.Waist);
                m.PlaySound(0x211);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
