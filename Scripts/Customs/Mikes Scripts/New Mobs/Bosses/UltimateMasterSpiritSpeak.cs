using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of John Dee")]
    public class UltimateMasterSpiritSpeak : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterSpiritSpeak()
            : base(AIType.AI_Mage)
        {
            Name = "John Dee";
            Title = "The Master of Spirits";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.SpiritSpeak, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;
            
            AddItem(new Robe(0x497)); // Dark robe
            AddItem(new Sandals());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x47E; // Gray hair
        }

        public UltimateMasterSpiritSpeak(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(CrystalSkull), typeof(NecromancersRobes) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(SpiritTome), typeof(SpectralLantern) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(SpiritTome), typeof(EtherealAltar) }; }
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

            c.DropItem(new PowerScroll(SkillName.SpiritSpeak, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MaxxiaScroll());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MaxxiaScroll());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: SpiritBlast(defender); break;
                    case 1: SoulShield(); break;
                    case 2: EtherealForm(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void SpiritBlast(Mobile target)
        {
            if (target != null)
            {
                DoHarmful(target);
                int damage = Utility.RandomMinMax(70, 90);

                AOS.Damage(target, this, damage, 0, 0, 100, 0, 0);

                target.FixedParticles(0x374A, 10, 30, 5054, EffectLayer.Head);
                target.PlaySound(0x1FB);
            }
        }

        public void SoulShield()
        {
            this.FixedParticles(0x375A, 10, 15, 5013, EffectLayer.Waist);
            this.PlaySound(0x1F7);

            this.VirtualArmorMod += 50; // Increases the armor temporarily
            Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerCallback(RemoveSoulShield));
        }

        private void RemoveSoulShield()
        {
            this.VirtualArmorMod -= 50;
        }

        public void EtherealForm()
        {
            this.Hidden = true;
            this.CantWalk = true;

            Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerCallback(EndEtherealForm));
        }

        private void EndEtherealForm()
        {
            this.Hidden = false;
            this.CantWalk = false;
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
