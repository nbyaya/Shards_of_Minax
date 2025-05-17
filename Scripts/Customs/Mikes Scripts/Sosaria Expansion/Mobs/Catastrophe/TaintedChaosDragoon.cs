using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a tainted chaos dragoon corpse")]
    public class TaintedChaosDragoon : BaseCreature
    {
        // Next time the dragoon may execute one of its unique abilities
        private DateTime m_NextAbility;

        [Constructable]
        public TaintedChaosDragoon() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.20, 0.50)
        {
            Name = "a tainted chaos dragoon";
            Body = 0x190;
            BaseSoundID = 0x2C8; // base attack sound (see overrides below)
            Hue = 2100; // A unique hue that marks its tainted corruption

            // Advanced stats for a world–threat level monster
            SetStr(300, 350);
            SetDex(100, 120);
            SetInt(80, 100);

            SetHits(300, 350);
            SetDamage(30, 35);

            // Damage distribution (mixing physical with elemental/energy damage)
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Cold, 10);
            SetDamageType(ResistanceType.Energy, 40);

            // High resistances befitting an advanced monster
            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 55, 65);

            // Skills emphasizing melee and magic defense
            SetSkill(SkillName.Fencing, 100.0, 110.0);
            SetSkill(SkillName.Healing, 90.0, 100.0);
            SetSkill(SkillName.Macing, 100.0, 110.0);
            SetSkill(SkillName.Anatomy, 90.0, 105.0);
            SetSkill(SkillName.MagicResist, 105.0, 120.0);
            SetSkill(SkillName.Swords, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);

            Fame = 10000;
            Karma = -10000;
            VirtualArmor = 50;
            Tamable = false;

            // Equip a tainted version of the melee weapon (custom items based on your clan’s design)
            BaseWeapon melee = null;
            switch (Utility.Random(3))
            {
                case 0: melee = new TaintedKryss(); break;
                case 1: melee = new TaintedBroadsword(); break;
                case 2: melee = new TaintedKatana(); break;
            }
            melee.Movable = false;
            AddItem(melee);

            // Equip a full set of "tainted" dragon armor pieces (these should be defined similarly to the standard items)
            DragonHelm helm = new DragonHelm();
            helm.Movable = false;
            AddItem(helm);

            DragonChest chest = new DragonChest();
            chest.Movable = false;
            AddItem(chest);

            DragonArms arms = new DragonArms();
            arms.Movable = false;
            AddItem(arms);

            DragonGloves gloves = new DragonGloves();
            gloves.Movable = false;
            AddItem(gloves);

            DragonLegs legs = new DragonLegs();
            legs.Movable = false;
            AddItem(legs);

            ChaosShield shield = new ChaosShield();
            shield.Movable = false;
            AddItem(shield);

            // Standard clothing accessories
            AddItem(new Shirt());
            AddItem(new Boots());

            // Drop some tainted scales on death (again a custom item)
            int amount = Utility.RandomMinMax(3, 5);
            AddItem(new TaintedScales(amount));

            // Set a special ability flag (if your shard supports this type of flag)

            m_NextAbility = DateTime.UtcNow + TimeSpan.FromSeconds(5);
        }

        public TaintedChaosDragoon(Serial serial) : base(serial)
        {
        }

        // SOUND OVERRIDES: Use modified sound identifiers to highlight its unique vocalizations.
        public override int GetIdleSound() { return 0x2D0; }
        public override int GetDeathSound() { return 0x2CD; }
        public override int GetHurtSound() { return 0x2D2; }
        public override int GetAttackSound() { return 0x2C9; }

        public override bool AutoDispel { get { return true; } }
        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool BardImmune { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.Gems, 10);
        }


        // Increase melee damage significantly against draconic foes.
        public override void AlterMeleeDamageTo(Mobile to, ref int damage)
        {
            if (to is Dragon || to is WhiteWyrm || to is SwampDragon || to is Drake ||
                to is Nightmare || to is Hiryu || to is LesserHiryu || to is Daemon)
                damage *= 3;
        }

        // Core AI: In combat, every few seconds the dragoon will randomly execute a unique ability.
        public override void OnActionCombat()
        {
            Mobile target = Combatant as Mobile;
            if (target == null || target.Deleted || target.Map != Map || !InRange(target, 20) || !CanBeHarmful(target) || !InLOS(target))
                return;

            if (DateTime.UtcNow >= m_NextAbility)
            {
                // Randomly choose one of the three unique abilities.
                switch (Utility.Random(3))
                {
                    case 0:
                        TaintedVoidBurst(target);
                        break;
                    case 1:
                        MalignantDrain(target);
                        break;
                    case 2:
                        CorruptedAura();
                        break;
                }
                m_NextAbility = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            }
        }

        //-------------------------------------------------------------------------
        // Unique Ability 1: Tainted Void Burst
        // An area attack that creates a chaotic explosion, damaging nearby foes
        // and applying a lethal poison effect.
        public void TaintedVoidBurst(Mobile target)
        {
            DoHarmful(target);

            Point3D loc = target.Location;
            Map map = Map;
            // Visual: a special effect with a tainted hue
            Effects.SendLocationEffect(loc, map, 0x374A, 30, 2100, 0);

            // Get all mobiles within a 3-tile radius.
            List<Mobile> mobiles = new List<Mobile>();
            IPooledEnumerable eable = map.GetMobilesInRange(loc, 3);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m))
                    mobiles.Add(m);
            }
            eable.Free();

            // Deal chaotic damage and afflict lethal poison.
            foreach (Mobile m in mobiles)
            {
                DoHarmful(m);
                int damage = Utility.RandomMinMax(50, 80);
                AOS.Damage(m, this, damage, 0, 50, 0, 0, 50);
                // Always check that m is a Mobile before applying poison.
                if (m != null)
                    m.ApplyPoison(this, Poison.Lethal);
            }
        }

        //-------------------------------------------------------------------------
        // Unique Ability 2: Malignant Drain
        // Drains life from the target while transferring part of that damage as healing
        // to the dragoon.
        public void MalignantDrain(Mobile target)
        {
            DoHarmful(target);
            // Show drain particles moving from the target to the dragoon.
            int damage = Utility.RandomMinMax(40, 60);
            AOS.Damage(target, this, damage, 0, 0, 0, 100, 0);
            this.Hits += damage / 2; // Heal self for half the damage inflicted.
        }

        //-------------------------------------------------------------------------
        // Unique Ability 3: Corrupted Aura
        // Releases a short–lived damaging aura that damages any foe within 2 tiles.
        public void CorruptedAura()
        {
            new CorruptedAuraTimer(this, 3).Start();
        }

        // Timer for Corrupted Aura: ticks every second for the given duration.
        private class CorruptedAuraTimer : Timer
        {
            private TaintedChaosDragoon m_Dragoon;
            private int m_Ticks;

            public CorruptedAuraTimer(TaintedChaosDragoon dragoon, int durationInSeconds)
                : base(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
            {
                m_Dragoon = dragoon;
                m_Ticks = durationInSeconds;
            }

            protected override void OnTick()
            {
                if (m_Dragoon.Deleted)
                {
                    Stop();
                    return;
                }

                IPooledEnumerable eable = m_Dragoon.Map.GetMobilesInRange(m_Dragoon.Location, 2);
                foreach (Mobile m in eable)
                {
                    if (m != m_Dragoon && m_Dragoon.CanBeHarmful(m))
                    {
                        m_Dragoon.DoHarmful(m);
                        int damage = Utility.RandomMinMax(10, 20);
                        AOS.Damage(m, m_Dragoon, damage, 0, 0, 0, 100, 0);
                    }
                }
                eable.Free();

                m_Ticks--;
                if (m_Ticks <= 0)
                    Stop();
            }
        }

        //-------------------------------------------------------------------------
        // When damaged by a spell, there is a chance to spawn a tainted minion.
        public override void OnDamagedBySpell(Mobile caster)
        {
            if (caster == this)
                return;
            if (Utility.RandomDouble() < 0.50) // 50% chance to spawn a minion on spell damage.
            {
                SpawnTaintedMinion(caster);
            }
        }

        // Spawns a tainted minion near the dragoon that will target the given Mobile.
        public void SpawnTaintedMinion(Mobile target)
        {
            Map map = Map;
            if (map == null)
                return;

            BaseCreature minion = new Orc();
            minion.Name = "tainted chaos minion";
            minion.SetDamage(20, 30);
            minion.SetHits(200, 250);
            minion.Team = this.Team;

            bool validLocation = false;
            Point3D loc = this.Location;
            for (int j = 0; !validLocation && j < 10; ++j)
            {
                int x = X + Utility.Random(3) - 1;
                int y = Y + Utility.Random(3) - 1;
                int z = map.GetAverageZ(x, y);

                if (map.CanFit(x, y, this.Z, 16, false, false))
                {
                    loc = new Point3D(x, y, this.Z);
                    validLocation = true;
                }
                else if (map.CanFit(x, y, z, 16, false, false))
                {
                    loc = new Point3D(x, y, z);
                    validLocation = true;
                }
            }

            if (validLocation)
            {
                minion.MoveToWorld(loc, map);
                minion.Combatant = target;
            }
        }



        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int) 0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
