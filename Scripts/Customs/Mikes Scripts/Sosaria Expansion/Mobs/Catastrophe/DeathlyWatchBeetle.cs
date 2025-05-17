using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a deathly watch beetle corpse")]
    public class DeathlyWatchBeetle : BaseCreature
    {
        private long _NextAuraTick;  // timer for aura ability
        private DateTime m_Delay;    // delay for periodic actions

        public override double HealChance { get { return 1.0; } }

        [Constructable]
        public DeathlyWatchBeetle() 
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Deathly Watch Beetle";
            Body = 242;                   // same body as the base beetle
            BaseSoundID = 0x4F2;          // base idle sound
            Hue = 1175;                   // a unique advanced hue


            SetStr(300, 350);
            SetDex(60, 80);
            SetInt(70, 90);

            SetHits(1200, 1400);
            SetMana(100);

            SetDamage(20, 35);
            // Split damage types: 50% physical and 50% poison.
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 70.1, 80.0);
            SetSkill(SkillName.Tactics, 80.1, 90.0);
            SetSkill(SkillName.Wrestling, 80.1, 90.0);
            SetSkill(SkillName.Anatomy, 50.1, 60.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            // Advanced monster – not tamable.
            Tamable = false;

            // Use a couple of standard abilities as placeholders.
            SetWeaponAbility(WeaponAbility.CrushingBlow);
            SetSpecialAbility(SpecialAbility.PoisonSpit);

            _NextAuraTick = Core.TickCount + 3000;
            m_Delay = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        public DeathlyWatchBeetle(Serial serial)
            : base(serial)
        {
        }

        public override int Hides { get { return 10; } }

        // Use the original deathwatch beetle sound codes.
        public override int GetAngerSound() { return 0x4F3; }
        public override int GetIdleSound() { return 0x4F2; }
        public override int GetAttackSound() { return 0x4F1; }
        public override int GetHurtSound() { return 0x4F4; }
        public override int GetDeathSound() { return 0x4F0; }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 3);
            AddLoot(LootPack.MedScrolls, 2);

            // Slight chance for an advanced carapace drop.
            if (Utility.RandomDouble() < 0.005)
            {
                PackItem(new DeathWatchCarapace());
            }
        }

        // In combat, randomly trigger one of the advanced abilities.
        public override void OnActionCombat()
        {
            if (!(Combatant is Mobile target) || target.Deleted || target.Map != Map ||
                !InRange(target, 20) || !CanBeHarmful(target) || !InLOS(target))
                return;

            int ability = Utility.Random(3);
            switch (ability)
            {
                case 0:
                    DoPlagueAura(target);
                    break;
                case 1:
                    DoPoisonSpit(target);
                    break;
                case 2:
                    DoNecroticBurst(target);
                    break;
            }
        }

        // Ability 1: Plague Aura – infect all nearby foes with lethal poison.
        public void DoPlagueAura(Mobile target)
        {
            if (!InRange(target.Location, 10))
                return;

            Effects.SendLocationEffect(target.Location, Map, 0x3728, 15, 1175, 0);
            Effects.PlaySound(target.Location, Map, 0x490); // poison effect sound

            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 3);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m) && InLOS(m))
                {
                    DoHarmful(m);
                    m.ApplyPoison(this, Poison.Lethal);
                }
            }
            eable.Free();
        }

        // Ability 2: Poison Spit – a ranged attack that deals poison damage.
        public void DoPoisonSpit(Mobile target)
        {
            if (!InRange(target.Location, 10))
                return;

            // Visual: send a moving poison projectile.
            MovingParticles(target, 0x379B, 10, 0, false, true, 1175, 0, 9534, 0, 0, EffectLayer.Head, 0x100);
            PlaySound(0x490);

            int damage = Utility.RandomMinMax(30, 50);
            DoHarmful(target);
            AOS.Damage(target, this, damage, 0, 0, 100, 0, 0);
            target.ApplyPoison(this, Poison.Deadly);
        }

        // Ability 3: Necrotic Burst – an area attack that deals damage and drains life.
        public void DoNecroticBurst(Mobile target)
        {
            if (!InRange(target.Location, 5))
                return;

            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 1175, 0);
            PlaySound(0x5C3);
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 2);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m) && InLOS(m))
                {
                    DoHarmful(m);
                    int burstDamage = Utility.RandomMinMax(40, 60);
                    AOS.Damage(m, this, burstDamage, 80, 0, 20, 0, 0);
                    // Heal self for half the damage dealt.
                    Hits += burstDamage / 2;
                }
            }
            eable.Free();
        }

        // When damaged by spells, has a chance to spawn a larva minion.
        public override void OnDamagedBySpell(Mobile caster)
        {
            if (caster == this)
                return;
            if (Utility.RandomDouble() < 0.25)
                SpawnLarva(caster);
        }

        public void SpawnLarva(Mobile caster)
        {
            Map map = Map;
            if (map == null)
                return;

            BaseCreature spawned = new DeathlyWatchLarva();
            spawned.Name = "Deathly Watch Larva";
            spawned.Team = this.Team;

            bool validLocation = false;
            Point3D loc = Location;

            for (int j = 0; !validLocation && j < 10; ++j)
            {
                int x = X + Utility.Random(3) - 1;
                int y = Y + Utility.Random(3) - 1;
                int z = map.GetAverageZ(x, y);

                if (validLocation = map.CanFit(x, y, Z, 16, false, false))
                    loc = new Point3D(x, y, Z);
                else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                    loc = new Point3D(x, y, z);
            }

            if (validLocation)
            {
                spawned.MoveToWorld(loc, map);
                spawned.Combatant = caster;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!(Combatant is Mobile target))
                return;

            if (Core.TickCount > _NextAuraTick)
            {
                if (Utility.RandomDouble() < 0.4)
                    DoPlagueAura(target);
                _NextAuraTick = Core.TickCount + 5000;
            }
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from is BaseCreature)
            {
                BaseCreature bc = from as BaseCreature;
                if (Utility.RandomDouble() < 0.7)
                    bc.Damage(damage, this); // Reflect damage back onto creatures (pets, etc.)
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
            writer.Write(_NextAuraTick);
            writer.Write(m_Delay);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            _NextAuraTick = reader.ReadLong();
            m_Delay = reader.ReadDateTime();
        }
    }

    // A minimal larva minion class for use with DeathlyWatchBeetle.
    public class DeathlyWatchLarva : BaseCreature
    {
        [Constructable]
        public DeathlyWatchLarva()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a deathly watch larva";
            Body = 242;          // using the beetle body for consistency (or pick a smaller body id)
            BaseSoundID = 0x4F2;
            Hue = 1175;

            SetStr(100, 120);
            SetDex(80, 100);
            SetInt(50, 60);

            SetHits(300, 350);
            SetDamage(10, 15);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 10, 20);

            VirtualArmor = 20;

            Tamable = false;
        }

        public DeathlyWatchLarva(Serial serial) : base(serial)
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
