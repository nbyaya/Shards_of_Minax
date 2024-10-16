using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Oda Nobunaga")]
    public class UltimateMasterBushido : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterBushido()
            : base(AIType.AI_Melee)
        {
            Name = "Oda Nobunaga";
            Title = "The Devil";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(500, 700);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(15000);
            SetMana(1000);

            SetDamage(40, 50);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Swords, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Parry, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 80;

            AddItem(new SamuraiHelm());
            AddItem(new PlateChest());
            AddItem(new PlateArms());
            AddItem(new PlateGorget());
            AddItem(new PlateGloves());
            AddItem(new PlateSuneate());
            AddItem(new NoDachi());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x96;

            AddItem(new LongHair(Utility.RandomNeutralHue()));
        }

        public UltimateMasterBushido(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(DualKatanas), typeof(SamuraiArmor) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(SamuraiSash), typeof(WarriorTome) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(SamuraiStatue), typeof(SwordDisplay) }; }
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

            c.DropItem(new PowerScroll(SkillName.Bushido, 200.0));

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
                    case 0: DualStrike(defender); break;
                    case 1: FocusSlash(defender); break;
                    case 2: Parry(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void DualStrike(Mobile defender)
        {
            if (defender != null)
            {
                DoHarmful(defender);
                AOS.Damage(defender, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);
                AOS.Damage(defender, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);

                defender.FixedParticles(0x3779, 1, 20, 97, 0x3F, 0, EffectLayer.Waist);
                defender.PlaySound(0x3B9);
            }
        }

        public void FocusSlash(Mobile defender)
        {
            if (defender != null)
            {
                DoHarmful(defender);
                AOS.Damage(defender, this, Utility.RandomMinMax(70, 90), 100, 0, 0, 0, 0);

                defender.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                defender.PlaySound(0x207);
            }
        }

        public void Parry()
        {
            if (Combatant != null)
            {
                this.VirtualArmorMod += 20;
                this.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                this.PlaySound(0x1F5);
                Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerCallback(RemoveParryEffect));
            }
        }

        private void RemoveParryEffect()
        {
            this.VirtualArmorMod -= 20;
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
