using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Merlin")]
    public class UltimateMasterMage : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterMage()
            : base(AIType.AI_Mage)
        {
            Name = "Merlin";
            Title = "The Ultimate Master of Magery";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            AddItem(new Robe(Utility.RandomBlueHue()));
            AddItem(new Sandals());

            HairItemID = 0x203B; // Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterMage(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(StaffOfMerlin), typeof(SpellbookOfAvalon) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(MagicWand), typeof(MysticalScroll) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(ArcaneSymbol), typeof(MagicCrystal) }; }
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

            c.DropItem(new PowerScroll(SkillName.Magery, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new StaffOfMerlin());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new SpellbookOfAvalon());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: ArcaneBlast(defender); break;
                    case 1: ManaShield(); break;
                    case 2: TimeWarp(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void ArcaneBlast(Mobile target)
        {
            if (target != null)
            {
                DoHarmful(target);
                int damage = Utility.RandomMinMax(60, 80);
                AOS.Damage(target, this, damage, 0, 0, 100, 0, 0);
                target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                target.PlaySound(0x307);
            }
        }

        public void ManaShield()
        {
            this.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
            this.PlaySound(0x1EA);
            // Implementing the actual mana shield logic is complex and would require overriding the damage system
            // This is a placeholder for visual effect and sound
        }

        public void TimeWarp()
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

                m.Frozen = true;
                Timer.DelayCall(TimeSpan.FromSeconds(5), delegate { m.Frozen = false; });
                m.SendMessage("You feel time slowing down around you!");

                m.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                m.PlaySound(0x1FA);
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
